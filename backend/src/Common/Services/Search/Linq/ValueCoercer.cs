using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Trippie.Common.Services.Search.Exception;

namespace Trippie.Common.Services.Search.Linq;

/// <summary>
/// Turns the raw <c>object?</c> a client sent (often a <see cref="JsonElement"/>
/// after System.Text.Json deserializes <c>object</c>) into a strongly-typed
/// <see cref="Expression"/> the LINQ pipeline can splice into a predicate.
///
/// <para>
/// Constants are wrapped in a <c>Holder&lt;T&gt;</c> closure so that EF Core
/// promotes them to query parameters (the SQL ends up <c>WHERE x = $1</c>, not
/// <c>WHERE x = 42</c>). Inlined constants would compile to a new plan-cache
/// entry per unique value — a real cost in Postgres on high-cardinality filters.
/// </para>
/// </summary>
internal static class ValueCoercer
{
    /// <summary>
    /// Build a parameter-bound <see cref="Expression"/> of type
    /// <paramref name="targetType"/> from <paramref name="raw"/>. Validation
    /// errors throw <see cref="SearchValidationException"/> (→ HTTP 400).
    /// </summary>
    public static Expression ToParameterExpression(object? raw, Type targetType)
    {
        var coerced = Coerce(raw, targetType);
        // Generic dispatch: build Holder&lt;T&gt;.Value where T == targetType.
        return (Expression)MakeHolderMI
            .MakeGenericMethod(targetType)
            .Invoke(null, [coerced])!;
    }

    /// <summary>
    /// Coerce a single value into <paramref name="targetType"/>. Returns null for
    /// null inputs. Returns the value unchanged if it's already assignment-compatible.
    /// </summary>
    public static object? Coerce(object? raw, Type targetType)
    {
        if (raw is null) return null;

        var unwrapped = UnwrapJson(raw);
        if (unwrapped is null) return null;

        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;
        if (underlying.IsInstanceOfType(unwrapped)) return unwrapped;

        try
        {
            if (underlying.IsEnum)
                return unwrapped is string s
                    ? Enum.Parse(underlying, s, ignoreCase: true)
                    : Enum.ToObject(underlying, unwrapped);

            if (underlying == typeof(Guid))
                return unwrapped is Guid g ? g : Guid.Parse(unwrapped.ToString()!);

            if (underlying == typeof(DateOnly))
                return DateOnly.Parse(unwrapped.ToString()!, CultureInfo.InvariantCulture);

            if (underlying == typeof(TimeOnly))
                return TimeOnly.Parse(unwrapped.ToString()!, CultureInfo.InvariantCulture);

            if (underlying == typeof(DateTime))
                return DateTime.Parse(unwrapped.ToString()!, CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind);

            if (underlying == typeof(DateTimeOffset))
                return DateTimeOffset.Parse(unwrapped.ToString()!, CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind);

            return Convert.ChangeType(unwrapped, underlying, CultureInfo.InvariantCulture);
        }
        catch (System.Exception ex)
        {
            throw new SearchValidationException(
                $"Cannot convert value '{raw}' to {targetType.Name}.", ex);
        }
    }

    /// <summary>
    /// Coerce an enumerable of raw values into a typed <c>List&lt;T&gt;</c>
    /// where <c>T == elementType</c>. EF Core / Npgsql translates
    /// <c>list.Contains(x.Col)</c> into <c>x.Col = ANY($1)</c> against a
    /// PostgreSQL array, which is index-friendly.
    /// </summary>
    public static object CoerceList(object? raw, Type elementType, string column)
    {
        if (raw is null)
            throw new SearchValidationException($"In/NotIn require a non-null collection for column: {column}");
        if (raw is string)
            throw new SearchValidationException($"In/NotIn require a list/array for column: {column}");

        IEnumerable items = raw switch
        {
            JsonElement el when el.ValueKind == JsonValueKind.Array => el.EnumerateArray(),
            IEnumerable e => e,
            _ => throw new SearchValidationException($"In/NotIn require a list/array for column: {column}"),
        };

        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (IList)Activator.CreateInstance(listType)!;
        foreach (var item in items)
        {
            list.Add(Coerce(item is JsonElement je ? UnwrapJson(je) : item, elementType));
        }
        return list;
    }

    private static object? UnwrapJson(object? value) => value switch
    {
        JsonElement el => el.ValueKind switch
        {
            JsonValueKind.String => el.GetString(),
            JsonValueKind.Number => el.TryGetInt64(out var l) ? l : el.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => el.GetRawText(),
        },
        _ => value,
    };

    private static readonly MethodInfo MakeHolderMI =
        typeof(ValueCoercer).GetMethod(nameof(MakeHolderExpression),
            BindingFlags.NonPublic | BindingFlags.Static)!;

    private static Expression MakeHolderExpression<T>(object? value)
    {
        var holder = new Holder<T>((T)value!);
        var c = Expression.Constant(holder);
        return Expression.Field(c, nameof(Holder<T>.Value));
    }

    private sealed class Holder<T>(T value)
    {
        // Public field (not property) is critical — EF Core's parameter extractor
        // recognises member access on closure-captured constants as a parameter.
        public readonly T Value = value;
}
}
