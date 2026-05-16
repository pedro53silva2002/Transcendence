namespace Trippie.Common.Services.Search.Abstractions;

public interface ISqlDialect
{
    char ParameterPrefix { get; }
    string Like(string column, string parameterName);
    string AnyOf(string column, string parameterName);
    string NoneOf(string column, string parameterName);
    string EscapeLike(string value);
    string FormatLimit(int limit);
}
