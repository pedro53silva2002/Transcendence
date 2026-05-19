using System.Linq.Expressions;
using System.Reflection;
using Trippie.Common.Services.Search.Model;

namespace Trippie.Common.Services.Search.Linq;

/// <summary>
/// Applies a list of <see cref="SortBinding{TEntity}"/> to an
/// <see cref="IQueryable{T}"/> using the strongly-typed OrderBy /
/// OrderByDescending / ThenBy / ThenByDescending overloads.
///
/// <para>
/// We can't call the typed overloads directly because the selector's value type
/// is only known at runtime (each sort can have a different
/// <c>TKey</c>). Reflection is used <i>once per sort step</i> to close the
/// generic — cheap relative to building the SQL and round-tripping to Postgres.
/// </para>
/// </summary>
internal static class OrderingApplier
{
    public static IOrderedQueryable<TEntity> Apply<TEntity>(
        IQueryable<TEntity> source,
        IReadOnlyList<SortBinding<TEntity>> sorts)
    {
        if (sorts.Count == 0)
            throw new InvalidOperationException(
                "OrderingApplier.Apply called with no sort bindings. " +
                "SearchQueryBuilder is supposed to inject a key tiebreaker before reaching this point.");

        IOrderedQueryable<TEntity>? ordered = null;
        for (var i = 0; i < sorts.Count; i++)
        {
            var s = sorts[i];
            var (member, valueType) = ExpressionUnboxer.Unbox(s.Selector.Body);

            // Rebuild the lambda with the unboxed body so EF Core sees
            // ORDER BY col rather than ORDER BY CAST(col AS object) — the
            // former produces sane SQL and matches whatever index is on col.
            var lambda = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(typeof(TEntity), valueType),
                member,
                s.Selector.Parameters[0]);

            var isFirst = i == 0;
            var desc = s.Direction == SortDirection.Desc;
            var methodName = isFirst
                ? (desc ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy))
                : (desc ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy));

            var openMI = QueryableMethods[methodName];
            var closed = openMI.MakeGenericMethod(typeof(TEntity), valueType);
            ordered = (IOrderedQueryable<TEntity>)closed.Invoke(null, [i == 0 ? source : ordered!, lambda])!;
        }

        return ordered!;
    }

    private static readonly Dictionary<string, MethodInfo> QueryableMethods =
        typeof(Queryable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetParameters().Length == 2
                && (m.Name == nameof(Queryable.OrderBy)
                    || m.Name == nameof(Queryable.OrderByDescending)
                    || m.Name == nameof(Queryable.ThenBy)
                    || m.Name == nameof(Queryable.ThenByDescending)))
            .ToDictionary(m => m.Name, m => m);
}
