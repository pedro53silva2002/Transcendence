namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// Pagination request from the client.
/// </summary>
/// <param name="PageSize">
/// How many rows the client wants. Will be clamped to <c>[1, SearchOptions.MaxPageSize]</c>
/// by the compiler — a malicious client can't request 1,000,000 rows.
/// </param>
/// <param name="EncodedCursor">
/// Opaque cursor produced by a previous response. Null for the first page.
/// Internally it's just base64url(JSON of the last row's sort columns), but the client
/// MUST treat it as opaque — we reserve the right to change the encoding.
/// </param>
public sealed record CursorPageRequest(int PageSize = 20, string? EncodedCursor = null);
