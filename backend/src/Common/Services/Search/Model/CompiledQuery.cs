namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// Result of compiling a <see cref="SearchPayload"/> against a schema: a ready-to-execute
/// SQL string plus its parameters and metadata needed to build the cursor for the next page.
/// </summary>
/// <param name="Sql">The full SQL — SELECT + WHERE + ORDER BY + LIMIT.</param>
/// <param name="Parameters">
/// Map of parameter names (without the @ prefix) to their values, ready to hand to Dapper.
/// We use <see cref="IReadOnlyDictionary{TKey,TValue}"/> so the executor can't mutate it.
/// </param>
/// <param name="Limit">
/// The LIMIT actually used. This is pageSize+1 — we fetch one extra row to detect whether
/// there is a next page WITHOUT a second COUNT query. If the result has Limit rows, we know
/// there is more; we trim the last and return HasNext=true.
/// </param>
/// <param name="SortFields">
/// The logical field names that participated in ORDER BY, IN ORDER. The executor uses this
/// list to extract the last row's values when encoding the next cursor.
/// </param>
public sealed record CompiledQuery(
    string Sql,
    IReadOnlyDictionary<string, object?> Parameters,
    int Limit,
    IReadOnlyList<string> SortFields);
