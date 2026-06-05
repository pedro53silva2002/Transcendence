using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;

namespace Trippie.Modules.Travel.Model;

public sealed class Trip(AppDbContext db)
{
	public required int Id { get; set; }
	public required string TripName { get; set; }
	public string? Description { get; set; }
	public required int Duration { get; set; }
	public required DateTime StartDate { get; set; }
	public required DateTime EndDate { get; set; }
	public required int Budget { get; set; }
	public required TripVisibility Visibility { get; set; }
	public required int CreatedBy { get; set; }
	public DateTime? CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }

	public TripCountry? TripCountries { get; set; }
	public ICollection<TripCity> TripCities { get; set; } = [];
	public static TripDto ToDto(Trip t) => new()
	{
		Id = t.Id,
		TripName = t.TripName,
		Description = t.Description,
		Duration = t.Duration,
		StartDate = t.StartDate,
		EndDate = t.EndDate,
		Budget = t.Budget,
		Visibility = t.Visibility,
		CreatedBy = t.CreatedBy,
		CreatedAt = t.CreatedAt ?? DateTime.UtcNow,
		UpdatedAt = t.UpdatedAt,
		CountryId = t.TripCountries?.CountryId ?? 0,
		CityIds = t.TripCities.Select(tc => tc.CityId).ToList(),
	};
}

public sealed class TripModel(AppDbContext db)
{
	public async Task<TripDto> CreateAsync(CreateTripDto dto, CancellationToken ct = default)
	{
		var trip = new Trip(db)
		{
			Id = 0,
			TripName = dto.TripName,
			Description = dto.Description,
			Duration = (dto.EndDate - dto.StartDate).Days,
			StartDate = dto.StartDate,
			EndDate = dto.EndDate,
			Budget = dto.Budget == 0 ? 0 : dto.Budget, // --- IGNORE ---
			Visibility = dto.Visibility == 0 ? TripVisibility.Public : dto.Visibility,
			CreatedBy = dto.CreatedBy,
			CreatedAt = DateTime.UtcNow,
		};

		db.Trips.Add(trip);
		await db.SaveChangesAsync(ct);

		db.TripCountries.Add(new TripCountry { TripId = trip.Id, CountryId = dto.CountryId });

		await db.SaveChangesAsync(ct);

		foreach (var cityId in dto.CityIds)
			db.TripCities.Add(new TripCity { TripId = trip.Id, CityId = cityId });

		await db.SaveChangesAsync(ct);
		return Trip.ToDto(trip);
	}

	public async Task<CursorPage<TripDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
	{
		var query = db.Trips
			.Include(t => t.TripCountries)
			.Include(t => t.TripCities);

		var res = await new SearchQueryBuilder<Trip>(query)
		.WithKey("id", x => x.Id)
		.AddFilters(payload.Filters, field => field.ToLowerInvariant() switch
		{
			"tripname" => x => x.TripName,
			"startdate" => x => x.StartDate,
			"createdat" => x => x.CreatedAt,
			"id" => x => x.Id,
			"visibility" => x => x.Visibility,
			_ => throw new SearchValidationException($"Unknown filter field '{field}'."),
		})
		.SetOrderBy(payload.Sort, field => field.ToLowerInvariant() switch
		{
			"tripname" => x => x.TripName,
			"duration" => x => x.Duration,
			"startdate" => x => x.StartDate,
			"createdat" => x => x.CreatedAt,
			"id" => x => x.Id,
			_ => throw new SearchValidationException($"Unsortable field '{field}'."),
		})
		.SetCursorPagination(payload.Page)
		.RunAsync(x => Trip.ToDto(x), ct);  // join data not loaded in search results

		return res;
	}

	public async Task<TripDto?> UpdateAsync(int userId, int id, UpdateTripDto dto, CancellationToken ct = default)
	{
		var query = db.Trips
			.Include(t => t.TripCountries)
			.Include(t => t.TripCities);
		var trip = await query.FirstOrDefaultAsync(t => t.Id == id, ct);
		if (trip is null) return null;

		if (dto.TripName is not null) trip.TripName = dto.TripName;
		if (dto.Visibility != trip.Visibility) trip.Visibility = dto.Visibility;
		if (dto.Description is not null) trip.Description = dto.Description;
		if (dto.StartDate != trip.StartDate) trip.StartDate = dto.StartDate;
		if (dto.EndDate != trip.EndDate) trip.EndDate = dto.EndDate;
		if ((dto.EndDate - dto.StartDate).Days != trip.Duration) trip.Duration = (dto.EndDate - dto.StartDate).Days;
		if (dto.Budget is not 0) trip.Budget = dto.Budget;
		trip.UpdatedAt = DateTime.UtcNow;

		db.Trips.Update(trip);

		// Full replace of join records
		if (trip.TripCountries?.CountryId != dto.CountryId)
		{
			await db.TripCountries.Where(tc => tc.TripId == id).ExecuteDeleteAsync(ct);
			await db.TripCities.Where(tc => tc.TripId == id).ExecuteDeleteAsync(ct);

			db.TripCountries.Add(new TripCountry { TripId = id, CountryId = dto.CountryId });

			foreach (var cityId in dto.CityIds)
				db.TripCities.Add(new TripCity { TripId = id, CityId = cityId });
		}
		else
		{
			foreach (var cityId in dto.CityIds)
			{
				if (!trip.TripCities.Any(tc => tc.CityId == cityId))
					db.TripCities.Add(new TripCity { TripId = id, CityId = cityId });
			}
		}

		await db.SaveChangesAsync(ct);

		return Trip.ToDto(trip);
	}

	public async Task<TripDto?> GetById(int id, CancellationToken ct = default)
	{
		var trip = await db.Trips
		.Include(t => t.TripCountries)
		.Include(t => t.TripCities)
		.FirstOrDefaultAsync(t => t.Id == id, ct);
		if (trip is null) return null;

		return Trip.ToDto(trip);
	}

	public async Task<bool> DeleteAsync(int userId, int id, CancellationToken ct = default)
	{
		var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id, ct);
		if (trip is null) return false;
		// Only allow deletion if user is creator of trip
		var rows = await db.Trips.Where(u => u.Id == id).ExecuteDeleteAsync(ct);
		return rows > 0;
	}
}