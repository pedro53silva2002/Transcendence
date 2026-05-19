namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// One element of an ORDER BY clause: which column, and which direction.
/// </summary>
/// <param name="Column">Logical field name (resolved against the schema).</param>
/// <param name="Direction">Asc by default — matches user expectation when omitted.</param>
public sealed record SortCriterion(string Column, SortDirection Direction = SortDirection.Asc);
