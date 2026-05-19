namespace Trippie.Common.Services.Search.Exception;

/// <summary>
/// Thrown when a client-supplied payload is invalid — unknown field, disallowed operator,
/// malformed cursor, etc. ASP.NET middleware should map this to <b>HTTP 400 Bad Request</b>.
/// </summary>
/// <remarks>
/// IMPORTANT: developer configuration mistakes (e.g. registering the same field twice)
/// should NOT throw this; they throw <see cref="InvalidOperationException"/> instead and
/// map to 500. Keeping the distinction is what lets the same try/catch in the middleware
/// produce the right HTTP status without having to inspect message text.
///
/// This uses C# 12's "primary constructor" syntax — the parameters in parentheses are
/// available everywhere in the class body, including in base() calls.
/// </remarks>
public sealed class SearchValidationException(string message, System.Exception? inner = null)
    : System.Exception(message, inner);
