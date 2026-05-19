using System.Linq.Expressions;
using System.Text.Json;
using Trippie.Common.Services.Search.Compilation;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Model;

namespace Trippie.Common.Services.Search.Linq;

/// <summary>
/// Turns a cursor + ordered sort list into a typed "give me rows strictly after
/// this one" predicate.
///
/// <para>
/// PostgreSQL supports row-constructor comparison —
/// <c>(a, b, c) &gt; ($1, $2, $3)</c> — but only when every column sorts in the
/// same direction. To stay correct for mixed-direction sorts (and avoid relying
/// on a feature EF Core's expression translator won't emit anyway), we expand
/// to the canonical "cascading OR-of-AND":
/// </para>
/// <code>
///   ( a &gt; v1 )
///   OR ( a = v1 AND b &lt; v2 )                       -- a asc, b desc
///   OR ( a = v1 AND b = v2 AND c &gt; v3 )            -- + key tiebreaker
/// </code>
/// <para>
/// Postgres still uses the composite index <c>(a, b, c)</c> to evaluate the
/// first disjunct, then short-circuits — same asymptotic cost as the row
/// comparison form.
/// </para>
/// </summary>
internal static class KeysetTranslator
{
    public static Expression<Func<TEntity, bool>>? Build<TEntity>(
        IReadOnlyList<SortBinding<TEntity>> sorts,
        CursorData cursor)
    {
        if (sorts.Count == 0) return null;

        var param = Expression.Parameter(typeof(TEntity), "x");

        // Resolve each sort's [member, typed-cursor-value] up front, in lockstep
        // with `sorts`. If even one cursor field is missing we treat the cursor
        // as invalid — refusing to silently page from the wrong place.
        var resolved = new (Expression Member, Expression CursorValue, SortDirection Dir)[sorts.Count];
        for (var i = 0; i < sorts.Count; i++)
        {
            var s = sorts[i];
            if (!cursor.Data.TryGetValue(s.LogicalName, out var rawElement))
                throw new SearchValidationException(
                    $"Cursor is missing required sort field '{s.LogicalName}'.");

            var member = ExpressionUnboxer.Rebind(
                ExpressionUnboxer.Unbox(s.Selector.Body).Inner,
                s.Selector.Parameters[0],
                param);

            var valueExpr = ValueCoercer.ToParameterExpression(rawElement, s.ValueType);
            resolved[i] = (member, valueExpr, s.Direction);
        }

        // Build the disjuncts. Disjunct i = (eq on 0..i-1) AND (strict-compare on i).
        Expression? whole = null;
        for (var i = 0; i < resolved.Length; i++)
        {
            Expression? conjunct = null;
            for (var j = 0; j < i; j++)
            {
                var eq = Expression.Equal(resolved[j].Member, resolved[j].CursorValue);
                conjunct = conjunct is null ? eq : Expression.AndAlso(conjunct, eq);
            }
            var strict = resolved[i].Dir == SortDirection.Asc
                ? Expression.GreaterThan(resolved[i].Member, resolved[i].CursorValue)
                : Expression.LessThan(resolved[i].Member, resolved[i].CursorValue);
            var disjunct = conjunct is null ? strict : Expression.AndAlso(conjunct, strict);

            whole = whole is null ? disjunct : Expression.OrElse(whole, disjunct);
        }

        return Expression.Lambda<Func<TEntity, bool>>(whole!, param);
    }

    /// <summary>
    /// Pulls the last-row sort values out of the dictionary the projection
    /// captured (see <see cref="SearchQueryBuilder{TEntity}"/>). Returns a map
    /// keyed by logical field name so <see cref="CursorCodec.Encode"/> can
    /// turn it into the opaque next-cursor string.
    /// </summary>
    public static IReadOnlyDictionary<string, object?> EncodeKeys<TEntity>(
        IReadOnlyList<SortBinding<TEntity>> sorts,
        object?[] lastRowKeys)
    {
        if (lastRowKeys.Length != sorts.Count)
            throw new InvalidOperationException(
                "Sort/key length mismatch — projection and sort plan got out of sync.");

        var dict = new Dictionary<string, object?>(sorts.Count, StringComparer.Ordinal);
        for (var i = 0; i < sorts.Count; i++)
            dict[sorts[i].LogicalName] = NormalizeForJson(lastRowKeys[i]);
        return dict;
    }

    // System.Text.Json handles most primitives, but DateOnly/TimeOnly/Guid need
    // to round-trip as strings so the next request can decode them back without
    // any special converters registered.
    private static object? NormalizeForJson(object? v) => v switch
    {
        null => null,
        DateOnly d => d.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
        TimeOnly t => t.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
        DateTime dt => dt.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
        DateTimeOffset dto => dto.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
        Guid g => g.ToString(),
        Enum e => e.ToString(),
        _ => v,
    };
}
