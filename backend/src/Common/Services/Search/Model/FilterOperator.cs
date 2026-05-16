namespace Trippie.Common.Services.Search.Model;

/// <summary>
/// All the comparison operators a client can use in a filter.
/// </summary>
/// <remarks>
/// These map 1:1 to the Java enum. Names use PascalCase here (C# convention)
/// instead of SCREAMING_SNAKE_CASE (Java convention).
///
/// The compiler turns each value into a SQL fragment. For example:
///   <list type="bullet">
///     <item><see cref="Equal"/>          -> "col = @p"</item>
///     <item><see cref="Contains"/>       -> "col ILIKE @p ESCAPE '\'"</item>
///     <item><see cref="In"/>             -> "col = ANY(@p)"  (Postgres)</item>
///   </list>
/// </remarks>
public enum FilterOperator
{
    // ----- standard comparisons -----
    Equal,              // col = @p
    NotEqual,           // col <> @p
    GreaterThan,        // col > @p
    GreaterThanOrEqual, // col >= @p
    LessThan,           // col < @p
    LessThanOrEqual,    // col <= @p

    // ----- semantic aliases for dates (same SQL as GT/LT, clearer in payloads) -----
    After,
    Before,

    // ----- range (requires BOTH 'value' and 'valueTo') -----
    Between, // col BETWEEN @p1 AND @p2

    // ----- string pattern matching (uses ILIKE on Postgres) -----
    Contains,   // %value%
    StartsWith, // value%
    EndsWith,   // %value

    // ----- null checks (no parameter is bound for these) -----
    IsNull,
    IsNotNull,

    // ----- list operators -----
    In,    // col = ANY(@p)
    NotIn  // col <> ALL(@p)
}
