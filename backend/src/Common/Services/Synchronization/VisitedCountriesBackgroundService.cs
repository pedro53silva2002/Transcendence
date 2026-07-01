using Trippie.Modules.Auth.Service;

namespace Trippie.Common.Services.Synchronization;

public sealed class VisitedCountriesSyncBackgroundService(
	IServiceScopeFactory scopeFactory,
	ILogger<VisitedCountriesSyncBackgroundService> logger) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await RunSyncAsync(stoppingToken);

		while (!stoppingToken.IsCancellationRequested)
		{
			var delay = GetDelayUntilNextRun();

			try
			{
				await Task.Delay(delay, stoppingToken);
			}
			catch (TaskCanceledException)
			{
				break;
			}

			await RunSyncAsync(stoppingToken);
		}
	}

	private async Task RunSyncAsync(CancellationToken ct)
	{
		using var scope = scopeFactory.CreateScope();
		var visitedCountriesService = scope.ServiceProvider.GetRequiredService<VisitedCountriesService>();
		
		try
		{
			logger.LogInformation("Starting visited countries sync background task.");
			var newlyAddedCount = await visitedCountriesService.SyncAllTripsAsync(ct);
			logger.LogInformation(
				"VisitedCountries background sync completed. {Count} new entries created",
				newlyAddedCount);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error occurred during visited countries sync background task.");
		}
	}

	private static TimeSpan GetDelayUntilNextRun()
	{
		var now = DateTime.UtcNow;
		var nextRun = now.Date.AddDays(1); //midnight UTC of the next day
		return nextRun - now;
	}
}
