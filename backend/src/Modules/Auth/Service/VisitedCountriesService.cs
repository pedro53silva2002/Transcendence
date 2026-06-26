using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Service;

public sealed class VisitedCountriesService(VisitedCountriesModel visitedCountriesModel)
{
	public async Task<SyncVisitedCountriesResultDto> SyncFromCompletedTripsAsync(
		int userId,
		CancellationToken ct = default)
	{
		if (userId <= 0)
			throw new ValidationException("userId", "User ID must be a positive integer.");

		var newCountryIds = await visitedCountriesModel.SyncCompletedTripsForUserAsync(userId, ct);
		return new SyncVisitedCountriesResultDto
		{
			NewlyAddedCount = newCountryIds.Count,
			NewlyAddedCountryIds = newCountryIds
		};
			
	}

	public async Task SyncSingleTripAsync(
		int userId,
		int countryId,
		int tripId,
		DateOnly tripEndDate,
		CancellationToken ct = default)
	{
		ValidateUserId(userId);

		if (countryId <= 0 || countryId > 241)
			throw new ValidationException("countryId", "Not a valid Country ID.");
		if (tripId <= 0)
			throw new ValidationException("tripId", "Trip ID must be a positive integer.");
		
		var today = DateOnly.FromDateTime(DateTime.UtcNow);

		if (tripEndDate >= today)
			return;
		
		await visitedCountriesModel.SyncFromTripsAsync(userId, countryId, tripId, ct);
	}

	public async Task<List<VisitedCountry>> GetVisitedCountriesAsync(int userId, CancellationToken ct = default)
	{
		ValidateUserId(userId);

		return await visitedCountriesModel.GetVisitedCountriesAsync(userId, ct);
	}

	public async Task<int> GetNumberOfVisitedCountriesAsync(int userId, CancellationToken ct = default)
	{
		ValidateUserId(userId);

		return await visitedCountriesModel.GetNumberOfVisitedCountriesAsync(userId, ct);
	}

	public async Task RemoveVisitedCountryAsync(int userId, int countryId, CancellationToken ct = default)
	{
		ValidateUserId(userId);

		if (countryId <= 0 || countryId > 241)
			throw new ValidationException("countryId", "Not a valid Country ID.");

		var removed = await visitedCountriesModel.RemoveManualVisitedCountryAsync(userId, countryId, ct);

		if (!removed)
			throw new NotFoundException("Not.Found", "Visited country entry not found.");
	}

	private static void ValidateUserId(int userId)
	{
		if (userId <= 0)
			throw new ValidationException("userId", "User ID must be a positive integer.");
	}
}