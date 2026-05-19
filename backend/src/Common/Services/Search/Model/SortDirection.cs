namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// Direction of an ORDER BY clause.
/// </summary>
public enum SortDirection
{
    /// <summary>Ascending (A-Z, 0-9, oldest first).</summary>
    Asc,

    /// <summary>Descending (Z-A, 9-0, newest first).</summary>
    Desc
}
