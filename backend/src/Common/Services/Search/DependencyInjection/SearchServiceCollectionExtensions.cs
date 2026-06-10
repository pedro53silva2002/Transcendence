using Microsoft.Extensions.DependencyInjection.Extensions;
using Trippie.Common.Services.Search.Compilation;

namespace Trippie.Common.Services.Search.DependencyInjection;

/// <summary>
/// Optional DI helpers for the EF-Core-backed search subsystem. The builder works
/// without DI — <c>new SearchQueryBuilder&lt;T&gt;(query)</c> uses library defaults —
/// but registering shared <see cref="SearchOptions"/> here lets you tweak page-size
/// limits from configuration without code changes.
/// </summary>
public static class SearchServiceCollectionExtensions
{
    /// <summary>
    /// Registers a shared singleton <see cref="SearchOptions"/>. Pass null for
    /// library defaults (page size 20, max 100).
    /// </summary>
    public static IServiceCollection AddSearch(
        this IServiceCollection services,
        SearchOptions? options = null)
    {
        services.TryAddSingleton(options ?? new SearchOptions());
        return services;
    }
}
