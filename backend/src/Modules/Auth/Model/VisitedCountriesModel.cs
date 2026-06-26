using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;

namespace Trippie.Modules.Auth.Model;

public sealed class VisitedCountry
{
	public required int UserId { get; set; }
	public required int CountryId { get; set; }
	public required DateTime AddedAt { get; set; }
	public required int SourceTripId { get; set; }
	public Country Country { get; set; } = null!;
	public User User { get; set; } = null!;
}

public sealed class VisitedCountriesModel(AppDbContext db)
{
	public async Task<bool> SyncFromTripsAsync(int userId, int countryId, int tripId, CancellationToken ct = default)
	{
		var alreadyVisted = await db.VisitedCountries
			.AsNoTracking()
			.AnyAsync(vc => vc.UserId == userId && vc.CountryId == countryId, ct);
		
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

	public async Task<List<int>> SyncCompletedTripsForUserAsync(int userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(now);

        // All completed trips where this user is a member.
        var completedTripCountries = await db.TripMembers
            .AsNoTracking()
            .Where(tm => tm.UserId == userId)
            .Join(
                db.Trips.Where(t => t.EndDate < today),
                tm => tm.TripId,
                t => t.Id,
                (tm, t) => t)
            .Join(
                db.TripCountries,
                t => t.Id,
                tc => tc.TripId,
                (t, tc) => new { TripId = t.Id, tc.CountryId })
            .ToListAsync(ct);

        if (completedTripCountries.Count == 0)
            return [];

        // Countries the user already has visited.
        var alreadyVisitedCountryIds = await db.VisitedCountries
            .AsNoTracking()
            .Where(vc => vc.UserId == userId)
            .Select(vc => vc.CountryId)
            .ToHashSetAsync(ct);

        var toInsert = completedTripCountries
            .Where(x => !alreadyVisitedCountryIds.Contains(x.CountryId))
            // One entry per country (a user may have multiple trips to the same country).
            .DistinctBy(x => x.CountryId)
            .Select(x => new VisitedCountry
            {
                UserId = userId,
                CountryId = x.CountryId,
                AddedAt = now,
                SourceTripId = x.TripId
            })
            .ToList();

        if (toInsert.Count == 0)
            return [];

        db.VisitedCountries.AddRange(toInsert);
        await db.SaveChangesAsync(ct);

        return toInsert.Select(x => x.CountryId).ToList();
    }

	public async Task<List<VisitedCountry>> GetVisitedCountriesAsync(int userId, CancellationToken ct = default)
	{
		return await db.VisitedCountries
			.AsNoTracking()
			.Include(vc => vc.Country)
			.Where(vc => vc.UserId == userId)
			.OrderByDescending(vc => vc.AddedAt)
			.ToListAsync(ct);
	}

	public async Task<int> GetNumberOfVisitedCountriesAsync(int userId, CancellationToken ct = default)
	{
		return await db.VisitedCountries
			.CountAsync(vc => vc.UserId == userId, ct);
	}

	public async Task<bool> RemoveManualVisitedCountryAsync(int userId, int countryId, CancellationToken ct = default)
	{
		var rows = await db.VisitedCountries
			.Where(vc => 
				vc.UserId == userId && 
				vc.CountryId == countryId)
			.ExecuteDeleteAsync(ct);

		return rows > 0;
	}

	public async Task<bool> IsCountryVisitedAsync(int userId, int countryId, CancellationToken ct = default)
	{
		return await db.VisitedCountries
			.AsNoTracking()
			.AnyAsync(vc => vc.UserId == userId && vc.CountryId == countryId, ct);
	}
}
