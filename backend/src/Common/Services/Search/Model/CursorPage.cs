namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// What an API endpoint returns to the client: a page of results plus the cursor
/// to fetch the next page.
/// </summary>
/// <typeparam name="T">The row type (e.g. <c>UserDto</c>).</typeparam>
/// <param name="Content">The actual rows of this page.</param>
/// <param name="NextCursor">
/// Opaque cursor to pass back on the next request. Null when there is no next page.
/// </param>
/// <param name="HasNext">True if more rows exist beyond this page.</param>
/// <param name="Size">Number of items actually returned in <see cref="Content"/>.</param>
/// <remarks>
/// We use <c>IReadOnlyList&lt;T&gt;</c> instead of <c>List&lt;T&gt;</c> because the
/// caller should not be modifying the result — exposing the concrete list would
/// invite bugs where someone calls <c>.Add()</c> after the fact.
/// </remarks>
public sealed record CursorPage<T>(
	IList<T> Content,
	string? NextCursor,
	bool HasNext,
	int Size);
