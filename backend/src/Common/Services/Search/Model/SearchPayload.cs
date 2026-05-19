namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// The full request body the client POSTs to a search endpoint.
/// </summary>
/// <param name="Filters">WHERE clause filters. Null/empty means "no filters".</param>
/// <param name="Sort">ORDER BY criteria. Null/empty means "default sort by key".</param>
/// <param name="Page">Pagination. Null means "first page with default size".</param>
/// <remarks>
/// JSON example:
/// <code>
/// {
///   "filters": [{ "column": "username", "operator": "Contains", "value": "john" }],
///   "sort":    [{ "column": "createdAt", "direction": "Desc" }],
///   "page":    { "pageSize": 20 }
/// }
/// </code>
/// All three fields are optional so the simplest valid payload is the empty object <c>{}</c>.
/// </remarks>
public sealed record SearchPayload(
    IReadOnlyList<FilterCriterion>? Filters = null,
    IReadOnlyList<SortCriterion>? Sort = null,
    CursorPageRequest? Page = null);
