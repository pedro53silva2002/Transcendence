using System.Globalization;
using System.Text.Json;
using Trippie.Common.Services.Search.Exception;

namespace Trippie.Common.Services.Search.Compilation;

/// <summary>
/// A single conversion rule: "given a raw boxed value (probably from JSON), produce
/// an instance of <see cref="TargetType"/>".
/// </summary>
public interface IValueConverter
{
    Type TargetType { get; }
    object? Convert(object? value);
}

/// <summary>
/// Replaces Java's <c>switch (targetType.getSimpleName())</c> string-based dispatch
/// with a type-keyed dictionary. Open-closed: register a converter for your own
/// strongly-typed ID record (e.g. <c>UserId(long Value)</c>) and the compiler
/// learns about it without any change to its own code.
/// </summary>
/// <remarks>
/// Built-in converters cover all the primitives + common BCL types (Guid, DateOnly,
/// TimeOnly, DateTimeOffset).
///
/// JSON nuance: when System.Text.Json deserializes <c>object</c>, numbers and dates
/// come back as <see cref="JsonElement"/>. The built-in converters all call
/// <see cref="UnwrapJson"/> first so the caller never has to worry about that.
/// </remarks>
public sealed class ValueConverterRegistry
{
    private readonly Dictionary<Type, IValueConverter> _converters;

    /// <summary>
    /// Constructs the registry. <paramref name="extra"/> converters are merged in
    /// AFTER the built-ins, so a custom converter for <c>Guid</c> would replace the default.
    /// </summary>
    public ValueConverterRegistry(IEnumerable<IValueConverter>? extra = null)
    {
        _converters = new Dictionary<Type, IValueConverter>
        {
            [typeof(string)] = new Lambda<string>(v => UnwrapJson(v)?.ToString()),
            [typeof(int)] = new Lambda<int>(v => System.Convert.ToInt32(UnwrapJson(v), CultureInfo.InvariantCulture)),
            [typeof(long)] = new Lambda<long>(v => System.Convert.ToInt64(UnwrapJson(v), CultureInfo.InvariantCulture)),
            [typeof(short)] = new Lambda<short>(v => System.Convert.ToInt16(UnwrapJson(v), CultureInfo.InvariantCulture)),
            [typeof(byte)] = new Lambda<byte>(v => System.Convert.ToByte(UnwrapJson(v), CultureInfo.InvariantCulture)),
            [typeof(double)] = new Lambda<double>(v => System.Convert.ToDouble(UnwrapJson(v), CultureInfo.InvariantCulture)),
            [typeof(float)] = new Lambda<float>(v => System.Convert.ToSingle(UnwrapJson(v), CultureInfo.InvariantCulture)),
            [typeof(decimal)] = new Lambda<decimal>(v => System.Convert.ToDecimal(UnwrapJson(v), CultureInfo.InvariantCulture)),
            [typeof(bool)] = new Lambda<bool>(v => System.Convert.ToBoolean(UnwrapJson(v), CultureInfo.InvariantCulture)),
            [typeof(Guid)] = new Lambda<Guid>(v => UnwrapJson(v) is Guid g ? g : Guid.Parse(UnwrapJson(v)!.ToString()!)),
            [typeof(DateOnly)] = new Lambda<DateOnly>(v => UnwrapJson(v) is DateOnly d ? d : DateOnly.Parse(UnwrapJson(v)!.ToString()!, CultureInfo.InvariantCulture)),
            [typeof(TimeOnly)] = new Lambda<TimeOnly>(v => UnwrapJson(v) is TimeOnly t ? t : TimeOnly.Parse(UnwrapJson(v)!.ToString()!, CultureInfo.InvariantCulture)),
            [typeof(DateTime)] = new Lambda<DateTime>(v => UnwrapJson(v) is DateTime dt ? dt : DateTime.Parse(UnwrapJson(v)!.ToString()!, CultureInfo.InvariantCulture)),
            [typeof(DateTimeOffset)] = new Lambda<DateTimeOffset>(v => UnwrapJson(v) is DateTimeOffset dto ? dto : DateTimeOffset.Parse(UnwrapJson(v)!.ToString()!, CultureInfo.InvariantCulture)),
        };

        if (extra is null) return;
        foreach (var c in extra) _converters[c.TargetType] = c;
    }

    /// <summary>
    /// Coerce <paramref name="value"/> into <paramref name="targetType"/>. Returns the
    /// original value unchanged if it's already the right type, null, or
    /// <see cref="object"/> (the "untyped" fallback).
    /// </summary>
    public object? Convert(object? value, Type targetType)
    {
        // Cheap fast paths — no work if there's nothing to do.
        if (value is null || targetType == typeof(object)) return value;
        if (targetType.IsInstanceOfType(value)) return value;

        // Unknown type? Don't crash — let the DB driver try. ADO.NET handles a lot of
        // implicit conversions and we don't want to be MORE restrictive than necessary.
        if (!_converters.TryGetValue(targetType, out var converter))
            return value;

        try { return converter.Convert(value); }
        catch (System.Exception ex)
        {
            // This is USER input; misuse becomes 400 Bad Request, not 500.
            throw new SearchValidationException(
                $"Cannot convert value '{value}' to {targetType.Name}.", ex);
        }
    }

    /// <summary>
    /// System.Text.Json deserializes <c>object</c> as <see cref="JsonElement"/>; pull
    /// the underlying primitive out so the rest of the pipeline can work with plain
    /// CLR values.
    /// </summary>
    private static object? UnwrapJson(object? value) => value switch
    {
        JsonElement el => el.ValueKind switch
        {
            JsonValueKind.String => el.GetString(),
            JsonValueKind.Number => el.TryGetInt64(out var l) ? l : el.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => el.GetRawText()
        },
        _ => value
    };

    /// <summary>
    /// Thin adapter that lets us register a converter as a lambda instead of a
    /// dedicated class. Private nested type because no one outside this class
    /// should know how converters are stored.
    /// </summary>
    private sealed class Lambda<T>(Func<object?, T?> fn) : IValueConverter
    {
        public Type TargetType => typeof(T);
        public object? Convert(object? value) => fn(value);
    }
}
