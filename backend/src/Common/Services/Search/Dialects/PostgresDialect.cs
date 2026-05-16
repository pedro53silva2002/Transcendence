using Trippie.Common.Services.Search.Abstractions;

namespace Trippie.Common.Services.Search.Dialects;

/// <summary>
/// PostgreSQL dialect — matches the syntax used by the original Java code.
/// </summary>
/// <remarks>
/// - Uses <c>@param</c> style: that's the convention for Dapper + Npgsql.
///   (Npgsql also accepts <c>:param</c> JDBC-style, but <c>@</c> is what Dapper
///   uses internally when binding anonymous-object parameters.)
/// - <c>ILIKE</c> is Postgres's case-insensitive LIKE.
/// - <c>= ANY(@list)</c> binds an array; Dapper turns a <c>List&lt;T&gt;</c> into a
///   Postgres array automatically.
/// </remarks>
public sealed class PostgresDialect : ISqlDialect
{
    public char ParameterPrefix => '@';

    public string Like(string column, string parameterName) =>
        // ESCAPE '\' tells Postgres which character is the escape inside the pattern.
        // We escape '%' and '_' ourselves with this same character (see EscapeLike).
        $"{column} ILIKE @{parameterName} ESCAPE '\\'";

    public string AnyOf(string column, string parameterName) =>
        $"{column} = ANY(@{parameterName})";

    public string NoneOf(string column, string parameterName) =>
        $"{column} <> ALL(@{parameterName})";

    public string EscapeLike(string value) =>
        // Order matters: escape the escape char FIRST, otherwise the replacements for
        // % and _ would themselves get re-escaped.
        value
            .Replace("\\", "\\\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_");

    public string FormatLimit(int limit) => $" LIMIT {limit}";
}
