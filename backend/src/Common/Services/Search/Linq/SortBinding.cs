using System.Linq.Expressions;
using Trippie.Common.Services.Search.Model;

namespace Trippie.Common.Services.Search.Linq;

/// <summary>
/// Resolved sort step: the caller-supplied selector for the column, the value
/// type recovered from it, and the requested direction. Built once per query
/// by <see cref="SearchQueryBuilder{TEntity}"/>; consumed by both ordering and
/// keyset (cursor) translators so the two stay in lockstep — any keyset
/// predicate must align column-for-column with the ORDER BY or the cursor
/// stops being meaningful.
/// </summary>
internal sealed record SortBinding<TEntity>(
    string LogicalName,
    Expression<Func<TEntity, object?>> Selector,
    Type ValueType,
    SortDirection Direction);
