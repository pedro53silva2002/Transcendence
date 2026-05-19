namespace Trippie.Common.Services.Search.Compilation;

/// <summary>
/// Tunables for the search subsystem. Bound from configuration via the
/// <see cref="Microsoft.Extensions.Options.IOptions{TOptions}"/> pattern, which means
/// you can override them per-environment in appsettings.json without code changes:
/// <code>
/// "Search": {
///   "DefaultPageSize": 25,
///   "MaxPageSize": 200
/// }
/// </code>
/// </summary>
/// <remarks>
/// <c>init</c> properties (not <c>set</c>) mean the value can be assigned in an object
/// initializer or by the options binder, but not changed afterwards. That makes an
/// instance effectively read-only once configured.
/// </remarks>
public sealed class SearchOptions
{
    /// <summary>Page size used when the client doesn't specify one (or sends 0/negative).</summary>
    public int DefaultPageSize { get; init; } = 20;

    /// <summary>
    /// Hard ceiling enforced server-side. A malicious client can't ask for 1,000,000 rows.
    /// </summary>
    public int MaxPageSize { get; init; } = 100;
}
