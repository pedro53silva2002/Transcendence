using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;

namespace Trippie.Modules.Auth.Model;

public sealed class VisitedCountry
{
	public long Id { get; set; }
	public required int UserId { get; set; }
	public required int CountryId { get; set; }
	public int? SourceTripId { get; set; }
	public required DateTime AddedAt { get; set; }
	public DateTime? DeletedAt { get; set; }
	public Country Country { get; set; } = null!;
	public User User { get; set; } = null!;
}

public sealed class VisitedCountriesModel(AppDbContext db)
{
	public async Task<bool> SyncFromTripsAsync(int userId, int countryId, int tripId, CancellationToken ct = default)
	{
		var alreadyVisted = await db.VisitedCountries
			.AsNoTracking()
			.AnyAsync(vc => vc.UserId == userId &&
				vc.SourceTripId == tripId, ct);

		if (alreadyVisted)
			return false;

		var entry = new VisitedCountry
		{
			UserId = userId,
			CountryId = countryId,
			SourceTripId = tripId,
			AddedAt = DateTime.UtcNow
		};

		db.VisitedCountries.Add(entry);
		await db.SaveChangesAsync(ct);
		return true;
	}

	public async Task<List<int>> SyncAllTripsAsync(CancellationToken ct = default)
		=> await SyncCoreAsync(userId: null, ct);

	public async Task<List<int>> SyncCompletedTripsForUserAsync(int userId, CancellationToken ct = default)
		=> await SyncCoreAsync(userId, ct);

	private async Task<List<int>> SyncCoreAsync(int? userId, CancellationToken ct = default)
	{
		var today = DateOnly.FromDateTime(DateTime.UtcNow);
		var newlyAddedCountryIds = new List<int>();

		// SyncA option: Getting new completed trips
		var toExpireQuery = db.Trips.Where(t => t.EndDate < today && !t.IsExpired);

		if (userId is not null)
			toExpireQuery = toExpireQuery.Where(t =>
				db.TripMembers.Any(tm => tm.TripId == t.Id && tm.UserId == userId));

		var newlyExpiredTripIds = await toExpireQuery.Select(t => t.Id).ToListAsync(ct);

		if (newlyExpiredTripIds.Count > 0)
		{
			var memberTripCountry = await db.TripMembers
				.Where(tm => newlyExpiredTripIds.Contains(tm.TripId))
				.Join(db.TripCountries,
					tm => tm.TripId,
					tc => tc.TripId,
					(tm, tc) => new { tm.UserId, tm.TripId, tc.CountryId })
				.Where(x => userId == null || x.UserId == userId)
				.ToListAsync(ct);

			if (memberTripCountry.Count > 0)
			{
				var tripIdsInBatch = memberTripCountry.Select(x => x.TripId).Distinct().ToList();

				// Existing rows for these (UserId, TripId) pairs — regardless of DeletedAt.
				var existingPairs = await db.VisitedCountries
					.Where(vc => vc.SourceTripId != null && tripIdsInBatch.Contains(vc.SourceTripId.Value))
					.Select(vc => new { vc.UserId, TripId = vc.SourceTripId!.Value })
					.ToListAsync(ct);
				var existingSet = existingPairs.Select(x => (x.UserId, x.TripId)).ToHashSet();

				var now = DateTime.UtcNow;
				var toInsert = memberTripCountry
					.Where(x => !existingSet.Contains((x.UserId, x.TripId)))
					.Select(x => new VisitedCountry
					{
						UserId = x.UserId,
						CountryId = x.CountryId,
						SourceTripId = x.TripId,
						AddedAt = now
					})
					.ToList();

				if (toInsert.Count > 0)
				{
					db.VisitedCountries.AddRange(toInsert);
					await db.SaveChangesAsync(ct);
					newlyAddedCountryIds.AddRange(toInsert.Select(x => x.CountryId));
				}
			}

			// IsExpired is written last so that a crash before this point leaves the trip
			// still un-expired. The next sync will re-process it; existingPairs guards
			// against duplicate inserts.
			await db.Trips
				.Where(t => newlyExpiredTripIds.Contains(t.Id))
				.ExecuteUpdateAsync(x => x.SetProperty(t => t.IsExpired, true), ct);
		}

		// syncB option: trips un-expiring
		var toUnexpireQuery = db.Trips.Where(t => t.EndDate >= today && t.IsExpired);
		if (userId is not null)
			toUnexpireQuery = toUnexpireQuery.Where(t =>
				db.TripMembers.Any(tm => tm.TripId == t.Id && tm.UserId == userId));

		var unexpiringTripIds = await toUnexpireQuery.Select(t => t.Id).ToListAsync(ct);

		if (unexpiringTripIds.Count > 0)
		{
			var deleteQuery = db.VisitedCountries
				.Where(vc => vc.SourceTripId != null
					&& unexpiringTripIds.Contains(vc.SourceTripId.Value)
					&& vc.DeletedAt == null);
			if (userId is not null)
				deleteQuery = deleteQuery.Where(vc => vc.UserId == userId);

			await deleteQuery.ExecuteDeleteAsync(ct);

			// IsExpired is written last so that a crash before this point leaves the trip
			// still expired. The next sync will re-run the delete (no-op) then clear the flag.
			await db.Trips
				.Where(t => unexpiringTripIds.Contains(t.Id))
				.ExecuteUpdateAsync(x => x.SetProperty(t => t.IsExpired, false), ct);
		}

		// syncC option: country reconciliation for already-expired trips
		var activeVisitedQuery = db.VisitedCountries
			.Where(vc => vc.DeletedAt == null && vc.SourceTripId != null);
		if (userId is not null)
			activeVisitedQuery = activeVisitedQuery.Where(vc => vc.UserId == userId);

		var mismatches = await activeVisitedQuery
			.Join(db.TripCountries,
				vc => vc.SourceTripId!.Value,
				tc => tc.TripId,
				(vc, tc) => new { vc.Id, vc.CountryId, CurrentCountryId = tc.CountryId })
			.Where(x => x.CountryId != x.CurrentCountryId)
			.ToListAsync(ct);

		// I will assume that the number of mismatches is small, so we can update them one by one.
		// otherwise, we should replace this with a single SQL update
		foreach (var m in mismatches)
		{
			await db.VisitedCountries
				.Where(vc => vc.Id == m.Id)
				.ExecuteUpdateAsync(x => x.SetProperty(vc => vc.CountryId, m.CurrentCountryId), ct);
		}

		return newlyAddedCountryIds;
	}

	public async Task<List<VisitedCountry>> GetVisitedCountriesAsync(int userId, CancellationToken ct = default)
	{
		return await db.VisitedCountries
			.AsNoTracking()
			.Include(vc => vc.Country)
			.Where(vc => vc.UserId == userId && vc.DeletedAt == null)
			.OrderByDescending(vc => vc.AddedAt)
			.ToListAsync(ct);
	}

	public async Task<List<VisitedCountry>> GetUniqueVisitedCountriesAsync(int userId, CancellationToken ct = default)
	{
		var countries = await db.VisitedCountries
			.AsNoTracking()
			.Include(vc => vc.Country)
			.Where(vc => vc.UserId == userId && vc.DeletedAt == null)
			.ToListAsync(ct);

		return countries
			.GroupBy(vc => vc.CountryId)
			.Select(g => g.First())
			.OrderByDescending(vc => vc.AddedAt)
			.ToList();

		// return await db.VisitedCountries
		// 	.AsNoTracking()
		// 	.Where(vc => vc.UserId == userId && vc.DeletedAt == null)
		// 	.GroupBy(vc => vc.CountryId)
		// 	.Select(g => g.First())
		// 	.Include(vc => vc.Country)
		// 	.ToListAsync(ct);
	}

	public async Task<int> GetNumberOfVisitedCountriesAsync(int userId, CancellationToken ct = default)
	{
		return await db.VisitedCountries
			.Where(vc => vc.UserId == userId && vc.DeletedAt == null)
			.Select(vc => vc.CountryId)
			.Distinct()
			.CountAsync(ct);
	}

	public async Task<bool> RemoveManualVisitedCountryAsync(int userId, int countryId, CancellationToken ct = default)
	{
		var deletedAt = DateTime.UtcNow;

		var affectedRows = await db.VisitedCountries
			.Where(vc => vc.UserId == userId &&
				vc.CountryId == countryId &&
				vc.DeletedAt == null)
			.ExecuteUpdateAsync(x => x.SetProperty(vc => vc.DeletedAt, deletedAt), ct);

		return affectedRows > 0;
	}

	public async Task<bool> DeleteVisitedCountryAsync(int userId, int tripId, CancellationToken ct = default)
	{
		var affectedRows = await db.VisitedCountries
		.Where(vc =>
			vc.UserId == userId &&
			vc.SourceTripId == tripId)
		.ExecuteDeleteAsync(ct);

		return affectedRows > 0;
	}

	public async Task<bool> IsCountryVisitedAsync(int userId, int countryId, CancellationToken ct = default)
	{
		return await db.VisitedCountries
			.AsNoTracking()
			.AnyAsync(vc => vc.UserId == userId && vc.CountryId == countryId && vc.DeletedAt == null, ct);
	}
}
