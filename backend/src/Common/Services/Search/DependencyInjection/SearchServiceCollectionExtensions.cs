using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Trippie.Common.Services.Search.Compilation;
using Trippie.Common.Services.Search.Abstractions;
using Trippie.Common.Services.Search.Dialects;

namespace Trippie.Common.Services.Search.DependencyInjection;

/// <summary>
/// Optional DI helpers. The search library is usable without any DI registration —
/// <see cref="SearchQueryBuilder{TSearch,TOrder}"/> uses sensible defaults
/// (Postgres dialect, default converters, default options) when constructed without
/// overrides. Call <see cref="AddSearch"/> if you want a single shared dialect /
/// converter registry / options instance across the app.
/// </summary>
public static class SearchServiceCollectionExtensions
{
    /// <summary>
    /// Register shared singletons consumers can inject and pass into the builder.
    /// </summary>
    /// <param name="services">DI container.</param>
    /// <param name="options">
    /// Override page-size defaults. Pass <c>null</c> for library defaults
    /// (20 default page size, 100 max). <see cref="SearchOptions"/> is immutable —
    /// build it with an object initializer.
    /// </param>
    public static IServiceCollection AddSearch(
        this IServiceCollection services,
        SearchOptions? options = null)
    {
        services.TryAddSingleton(options ?? new SearchOptions());
        services.TryAddSingleton<ISqlDialect, PostgresDialect>();
        services.TryAddSingleton<ValueConverterRegistry>();
        return services;
    }
}
