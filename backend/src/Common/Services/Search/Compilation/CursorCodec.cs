using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Trippie.Common.Services.Search.Exception;

namespace Trippie.Common.Services.Search.Compilation;

/// <summary>
/// Opaque payload inside a cursor: a map of logical field name -> last row's value
/// as a JSON element.
/// </summary>
/// <remarks>
/// <see cref="JsonElement"/> as the value type lets us round-trip arbitrary JSON
/// scalars (string, number, bool, null) without picking a CLR type at encode time —
/// the matching converter will do that when the cursor is decoded server-side.
/// </remarks>
public sealed record CursorData(IReadOnlyDictionary<string, JsonElement> Data);

/// <summary>
/// Source-generated JSON metadata for <see cref="CursorData"/>. This is .NET's
/// reflection-free serialization story: at build time the compiler emits typed
/// (de)serializers, so the runtime never has to scan types via reflection.
/// Result: faster startup, smaller working set, AOT-compilable.
/// </summary>
[JsonSerializable(typeof(CursorData))]
internal partial class CursorJsonContext : JsonSerializerContext { }

/// <summary>
/// Stateless helpers to encode/decode a cursor string. Static is fine here because
/// there is no per-instance state and no I/O — the same input always produces the
/// same output.
/// </summary>
public static class CursorCodec
{
    /// <summary>
    /// Build a base64url-encoded cursor from a map of field-name → value.
    /// Returns null if the input is null/empty so callers can use the result directly
    /// as <c>nextCursor</c>.
    /// </summary>
    public static string? Encode(IReadOnlyDictionary<string, object?>? values)
    {
        if (values is null || values.Count == 0) return null;

        // Convert plain CLR values into JsonElement so they round-trip through the
        // strongly-typed CursorData record.
        var asJson = new Dictionary<string, JsonElement>(values.Count, StringComparer.Ordinal);
        foreach (var (k, v) in values)
        {
            asJson[k] = JsonSerializer.SerializeToElement(v);
        }

        var bytes = JsonSerializer.SerializeToUtf8Bytes(
            new CursorData(asJson),
            CursorJsonContext.Default.CursorData);

        // Base64Url (new in .NET 9) is URL-safe (uses '-' and '_' instead of '+' / '/')
        // and emits no padding by default. Perfect for embedding in URLs without
        // percent-encoding.
        return Base64Url.EncodeToString(bytes);
    }

    /// <summary>
    /// Decode the opaque cursor back into <see cref="CursorData"/>. Throws
    /// <see cref="SearchValidationException"/> (→ HTTP 400) for any malformed input.
    /// </summary>
    public static CursorData? Decode(string? encoded)
    {
        if (string.IsNullOrWhiteSpace(encoded)) return null;
        try
        {
            // Base64Url.DecodeFromChars accepts a ReadOnlySpan&lt;char&gt;, so passing
            // the string directly is allocation-free.
            var bytes = Base64Url.DecodeFromChars(encoded);
            return JsonSerializer.Deserialize(bytes, CursorJsonContext.Default.CursorData);
        }
        catch (System.Exception ex) when (ex is not SearchValidationException)
        {
            // Wrap ANY other exception (FormatException, JsonException, etc.) as a
            // validation error. The original is preserved as inner for logging.
            throw new SearchValidationException($"Invalid cursor: {encoded}", ex);
        }
    }
}
