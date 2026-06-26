using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.Authentication.Security;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;

namespace Trippie.Modules.Auth.Model;

public sealed class VisitedCountry
{
	public required int UserId { get; set; }
	public required int CountryId { get; set; }
	public required DateTime AddedAt { get; set; }
	public Country Country { get; set; } = null!;
	public User User { get; set; } = null!;
}

public sealed class VisitedCountriesModel(AppDbContext db)
{
	public async Task<VisitedCountry> AddVisitedCountryAsync(int userId, int countryId, CancellationToken ct = default)
	{
		var visitedCountry = new VisitedCountry
		{
			UserId = userId,
			CountryId = countryId,
			AddedAt = DateTime.UtcNow
		};

		try
		{
			db.VisitedCountries.Add(visitedCountry);
			await db.SaveChangesAsync(ct);
		}
		catch (DbUpdateException)
		{
			throw new InvalidOperationException("Country already visited by the user.");
		}

		return visitedCountry;
	}

	public async Task<List<VisitedCountry>> GetVisitedCountriesAsync(int userId, CancellationToken ct = default)
	{
		return await db.VisitedCountries
			.Include(vc => vc.Country)
			.Where(vc => vc.UserId == userId)
			.ToListAsync(ct);
	}

	public async Task<int> GetNumberOfVisitedCountriesAsync(int userId, CancellationToken ct = default)
	{
		return await db.VisitedCountries
			.CountAsync(vc => vc.UserId == userId, ct);
	}

	public async Task RemoveVisitedCountryAsync(int userId, int countryId, CancellationToken ct = default)
	{
		await db.VisitedCountries
			.Where(vc => vc.UserId == userId && vc.CountryId == countryId)
			.ExecuteDeleteAsync(ct);
	}

	public async Task<bool> IsCountryVisitedAsync(int userId, int countryId, CancellationToken ct = default)
	{
		return await db.VisitedCountries
			.AnyAsync(vc => vc.UserId == userId && vc.CountryId == countryId, ct);
	}
}
