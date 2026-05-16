using System.Globalization;

namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// Accumulator for the named parameters of a query being built.
/// </summary>
/// <remarks>
/// This replaces Java's <c>generateParamName</c> + manual collision loop. You give it
/// a column hint (e.g. "u.username") and a value; it returns the unique parameter name
/// it picked (e.g. "u_username_p"). If you call it again for the same column, it appends
/// a counter ("u_username_p0", "u_username_p1"...).
///
/// Notes on style:
/// - Stored as a <c>Dictionary</c> (mutable) because that's what an accumulator IS.
///   Wrapping a mutable accumulator in record/immutability ceremony would be silly.
/// - <c>StringComparer.Ordinal</c> for dictionary keys: case-sensitive, no culture
///   sensitivity, fastest possible lookup. Parameter names are always developer-controlled
///   ASCII identifiers.
/// - <c>Snapshot()</c> returns a read-only view used by <see cref="CompiledQuery"/> so
///   the executor cannot accidentally mutate our internal state.
/// </remarks>
public sealed class SqlParameterSet
{
    private readonly Dictionary<string, object?> _parameters = new(StringComparer.Ordinal);

    /// <summary>Returns an immutable view of the parameters collected so far.</summary>
    public IReadOnlyDictionary<string, object?> Snapshot() => _parameters;

    /// <summary>
    /// Adds a value under a name derived from <paramref name="columnHint"/>, choosing
    /// a unique suffix automatically. Returns the name actually used.
    /// </summary>
    /// <param name="columnHint">
    /// Often the SQL column ("u.username"). Dots are turned into underscores so the
    /// resulting name is a valid parameter identifier.
    /// </param>
    /// <param name="value">The value to bind. Conversion to the right CLR type should
    /// already have happened before calling this.</param>
    public string Add(ReadOnlySpan<char> columnHint, object? value)
    {
        var name = MakeUnique(columnHint);
        _parameters[name] = value;
        return name;
    }

    /// <summary>
    /// Adds a value under a caller-specified name. Used for fixed conditions and
    /// cursor parameters where the name needs to match a fragment already written.
    /// </summary>
    public void AddNamed(string name, object? value) => _parameters[name] = value;

    private string MakeUnique(ReadOnlySpan<char> hint)
    {
        // We sanitize on the stack to avoid allocating a temporary string-builder for
        // a name like "u.username" -> "u_username". For short identifiers this is a
        // micro-optimization, but on a hot path (many filters per query) it adds up.
        Span<char> buffer = stackalloc char[hint.Length];
        for (var i = 0; i < hint.Length; i++)
        {
            // Replace '.' with '_' so "u.username" becomes a valid parameter id.
            buffer[i] = hint[i] == '.' ? '_' : hint[i];
        }
        var basePart = new string(buffer);
        var candidate = $"{basePart}_p";

        // Fast path: first time we see this column, no collision.
        if (!_parameters.ContainsKey(candidate)) return candidate;

        // Slow path: probe with an incrementing suffix.
        // CultureInfo.InvariantCulture: never let regional settings change the digits.
        for (var i = 0; ; i++)
        {
            candidate = string.Create(CultureInfo.InvariantCulture, $"{basePart}_p{i}");
            if (!_parameters.ContainsKey(candidate)) return candidate;
        }
    }
}
