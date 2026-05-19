using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Trippie.Common.Services.GlobalExceptionHandler.Config;
using Trippie.Common.Services.GlobalExceptionHandler.Mappings;
using Trippie.Common.Services.GlobalExceptionHandler.Middleware;

namespace Trippie.Common.Services.GlobalExceptionHandler.DependencyInjection;

public static class ExceptionHandlingServiceCollectionExtensions
{
	public static IServiceCollection AddExceptionHandling(
		this IServiceCollection services,
		IConfiguration configuration
	)
	{
		services.Configure<ErrorHandlingOptions>(configuration.GetSection(ErrorHandlingOptions.SectionName));
		services.AddSingleton<IExceptionMapper, DomainExceptionMapper>();
		services.AddSingleton<IExceptionMapper, FrameworkExceptionMapper>();
		services.AddSingleton<IExceptionMapper, FallbackExceptionMapper>();

		services.TryAddSingleton<ExceptionMapperPipeline>();

		return services;
	}

	public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
	{
		app.UseMiddleware<CorrelationIdMiddleware>();
		app.UseMiddleware<GlobalExceptionMiddleware>();
		return app;
	}
}
