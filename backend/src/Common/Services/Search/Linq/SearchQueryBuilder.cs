using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trippie.Common.Services.Search.Compilation;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Model;

namespace Trippie.Common.Services.Search.Linq;

/// <summary>
/// Fluent, EF-Core-backed search pipeline. Composes filters, ordering, fixed
/// conditions and cursor pagination over an <see cref="IQueryable{TEntity}"/>
/// and materialises into a strongly-typed <see cref="CursorPage{TDto}"/>.
///
/// <para>
/// The wire contract is unchanged from the Dapper-era builder
/// (<see cref="SearchPayload"/> with string-keyed filters and sorts). The
/// difference is the <i>server-side</i> mapping: instead of returning a SQL
/// column string from the resolver lambda, callers return a typed member
/// selector and EF Core handles dialect, parameter binding, and SQL emission.
/// </para>
///
/// <para>Typical use:</para>
/// <code>
/// var result = await new SearchQueryBuilder&lt;Trip&gt;(db.Trips)
///     .WithKey("id", x =&gt; x.Id)
///     .Where(x =&gt; x.OwnerId == currentUserId)
///     .AddFilters(payload.Filters, field =&gt; field.ToLowerInvariant() switch
///     {
///         "title"     =&gt; x =&gt; x.Title,
///         "startDate" =&gt; x =&gt; x.StartDate,
///         "status"    =&gt; x =&gt; x.Status,
///         _ =&gt; throw new SearchValidationException($"Unknown field {field}")
///     })
///     .SetOrderBy(payload.Sort, f =&gt; f.ToLowerInvariant() switch
///     {
///         "startDate" =&gt; x =&gt; x.StartDate,
///         "title"     =&gt; x =&gt; x.Title,
///         _ =&gt; throw new SearchValidationException($"Unsortable field {f}")
///     })
///     .SetCursorPagination(payload.Page)
///     .RunAsync(x =&gt; new TripDto(x.Id, x.Title, x.StartDate, x.Status), ct);
/// </code>
/// </summary>
public sealed class SearchQueryBuilder<TEntity> where TEntity : class
{
    private readonly IQueryable<TEntity> _source;
    private readonly SearchOptions _options;
    private readonly List<Expression<Func<TEntity, bool>>> _wheres = new();
    private readonly List<SortBinding<TEntity>> _sorts = new();
    private CursorPageRequest? _page;
    private (string Name, Expression<Func<TEntity, object?>> Selector, Type ValueType)? _key;

    public SearchQueryBuilder(IQueryable<TEntity> source, SearchOptions? options = null)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _options = options ?? new SearchOptions();
    }

    /// <summary>
    /// Designates the stable tiebreaker column. Required — cursor pagination
    /// only guarantees "exactly one row per cursor" when the final ORDER BY
    /// column is unique. Conventionally the primary key.
    /// </summary>
    public SearchQueryBuilder<TEntity> WithKey(
        string logicalName,
        Expression<Func<TEntity, object?>> keySelector)
    {
        var (_, valueType) = ExpressionUnboxer.Unbox(keySelector.Body);
        _key = (logicalName, keySelector, valueType);
        return this;
    }

    /// <summary>
    /// Append a hardcoded WHERE clause. Use for tenant isolation, soft-delete
    /// filters, or any condition the client doesn't control. Stacks: each call
    /// adds another AND-ed predicate.
    /// </summary>
    public SearchQueryBuilder<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        _wheres.Add(predicate);
        return this;
    }

    /// <summary>
    /// Translate every <see cref="FilterCriterion"/> in <paramref name="filters"/>
    /// into a predicate. <paramref name="resolve"/> maps the logical column name
    /// (what the client sent) to a typed member selector. Throwing
    /// <see cref="SearchValidationException"/> from the resolver is the right
    /// way to reject unknown / disallowed fields.
    /// </summary>
    public SearchQueryBuilder<TEntity> AddFilters(
        IReadOnlyList<FilterCriterion>? filters,
        Func<string, Expression<Func<TEntity, object?>>> resolve)
    {
        if (filters is null || filters.Count == 0) return this;
        foreach (var f in filters)
        {
            if (string.IsNullOrWhiteSpace(f.Column))
                throw new SearchValidationException("Filter column cannot be empty.");
            var selector = resolve(f.Column);
            _wheres.Add(FilterTranslator.Build(f, selector));
        }
        return this;
    }

    /// <summary>
    /// Resolve and stash each <see cref="SortCriterion"/>. Order is preserved —
    /// the first criterion becomes the OrderBy, the rest become ThenBy.
    /// </summary>
    public SearchQueryBuilder<TEntity> SetOrderBy(
        IReadOnlyList<SortCriterion>? sorts,
        Func<string, Expression<Func<TEntity, object?>>> resolve)
    {
        _sorts.Clear();
        if (sorts is null) return this;
        foreach (var s in sorts)
        {
            if (string.IsNullOrWhiteSpace(s.Column))
                throw new SearchValidationException("Sort column cannot be empty.");
            var selector = resolve(s.Column);
            var (_, valueType) = ExpressionUnboxer.Unbox(selector.Body);
            _sorts.Add(new SortBinding<TEntity>(s.Column, selector, valueType, s.Direction));
        }
        return this;
    }

    public SearchQueryBuilder<TEntity> SetCursorPagination(CursorPageRequest? page)
    {
        _page = page;
        return this;
    }

    /// <summary>
    /// Materialise the page. <paramref name="projection"/> runs server-side —
    /// EF Core compiles it into a SELECT list, so only the columns you actually
    /// project come back. Defaults of <see cref="AsNoTracking"/> + cursor-bound
    /// LIMIT keep the per-call cost flat regardless of table size.
    /// </summary>
    public async Task<CursorPage<TDto>> RunAsync<TDto>(
        Expression<Func<TEntity, TDto>> projection,
        CancellationToken ct = default)
    {
        if (_key is null)
            throw new InvalidOperationException(
                $"{nameof(SearchQueryBuilder<TEntity>)} requires .WithKey(...) before .RunAsync().");

        // Inject the key as a final tiebreaker unless the caller already
        // ordered by it explicitly. Keep the caller's direction in that case.
        var sortPlan = BuildSortPlan(_key.Value);

        // Compose the queryable.
        var q = _source.AsNoTracking();
        foreach (var w in _wheres) q = q.Where(w);

        if (!string.IsNullOrWhiteSpace(_page?.EncodedCursor))
        {
            var cursor = CursorCodec.Decode(_page.EncodedCursor)
                ?? throw new SearchValidationException("Cursor decoded to empty payload.");
            var keysetPredicate = KeysetTranslator.Build(sortPlan, cursor);
            if (keysetPredicate is not null) q = q.Where(keysetPredicate);
        }

        var ordered = OrderingApplier.Apply(q, sortPlan);

        var requested = _page?.PageSize ?? _options.DefaultPageSize;
        if (requested <= 0) requested = _options.DefaultPageSize;
        var size = Math.Clamp(requested, 1, _options.MaxPageSize);

        // Project to (dto, sort-key array). Each key is read directly from
        // entity columns so EF Core extends the SELECT list rather than
        // forcing a second round-trip.
        var rowProjection = BuildRowProjection(projection, sortPlan);

        // Fetch size + 1 so we can tell "is there a next page" without a
        // COUNT(*) — that extra query would O(table) for every search and
        // defeat the whole point of cursor pagination.
        var rows = await ordered
            .Select(rowProjection)
            .Take(size + 1)
            .ToListAsync(ct)
            .ConfigureAwait(false);

        var hasNext = rows.Count > size;
        if (hasNext) rows.RemoveAt(rows.Count - 1);

        var content = new TDto[rows.Count];
        for (var i = 0; i < rows.Count; i++) content[i] = rows[i].Dto;

        string? nextCursor = null;
        if (hasNext && rows.Count > 0)
        {
            var lastKeys = rows[^1].Keys;
            var encoded = KeysetTranslator.EncodeKeys(sortPlan, lastKeys);
            nextCursor = CursorCodec.Encode(encoded);
        }

        return new CursorPage<TDto>(content, nextCursor, hasNext, content.Length);
    }

    private List<SortBinding<TEntity>> BuildSortPlan(
        (string Name, Expression<Func<TEntity, object?>> Selector, Type ValueType) key)
    {
        var plan = new List<SortBinding<TEntity>>(_sorts.Count + 1);
        var includesKey = false;
        foreach (var s in _sorts)
        {
            plan.Add(s);
            if (string.Equals(s.LogicalName, key.Name, StringComparison.Ordinal))
                includesKey = true;
        }
        if (!includesKey)
        {
            plan.Add(new SortBinding<TEntity>(
                key.Name, key.Selector, key.ValueType, SortDirection.Asc));
        }
        return plan;
    }

    /// <summary>
    /// Build <c>x =&gt; new ProjectionRow&lt;TDto&gt;(projection(x), new object?[]
    /// { x.sort1, x.sort2, ... })</c> as a single expression EF Core can
    /// translate into one SELECT.
    /// </summary>
    private static Expression<Func<TEntity, ProjectionRow<TDto>>> BuildRowProjection<TDto>(
        Expression<Func<TEntity, TDto>> projection,
        IReadOnlyList<SortBinding<TEntity>> sorts)
    {
        var param = Expression.Parameter(typeof(TEntity), "x");
        var dtoBody = ExpressionUnboxer.Rebind(projection.Body, projection.Parameters[0], param);

        var keyExprs = new Expression[sorts.Count];
        for (var i = 0; i < sorts.Count; i++)
        {
            var s = sorts[i];
            var member = ExpressionUnboxer.Rebind(
                ExpressionUnboxer.Unbox(s.Selector.Body).Inner,
                s.Selector.Parameters[0],
                param);

            // Box back to object so the runtime array element type stays uniform.
            // EF Core sees this as "select these scalar columns, then construct
            // a client-side array" — which is exactly what we want.
            keyExprs[i] = member.Type.IsValueType
                ? Expression.Convert(member, typeof(object))
                : Expression.Convert(member, typeof(object));
        }
        var keyArray = Expression.NewArrayInit(typeof(object), keyExprs);

        var ctor = typeof(ProjectionRow<TDto>).GetConstructors()[0];
        var newRow = Expression.New(ctor, dtoBody, keyArray);

        return Expression.Lambda<Func<TEntity, ProjectionRow<TDto>>>(newRow, param);
    }
}

/// <summary>
/// Materialised row carrying the projected DTO and the values of every column
/// that participated in ORDER BY. The latter are the inputs to the next cursor.
/// Internal-by-design: callers only ever see <see cref="CursorPage{TDto}"/>.
/// </summary>
public sealed record ProjectionRow<TDto>(TDto Dto, object?[] Keys);
