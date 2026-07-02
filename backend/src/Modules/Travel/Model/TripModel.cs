using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Modules.Auth.Dtos;

namespace Trippie.Modules.Travel.Model;

public sealed class Trip()
{
	public required int Id { get; set; }
	public required string TripName { get; set; }
	public string? Description { get; set; }
	public required int Duration { get; set; }
	public required DateOnly StartDate { get; set; }
	public required DateOnly EndDate { get; set; }
	public required int Budget { get; set; }
	public required TripVisibility Visibility { get; set; }
	public required int CreatedBy { get; set; }
	public required bool IsExpired { get; set; }
	public DateTime? CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public TripCountry? TripCountries { get; set; }
	public ICollection<TripCity> TripCities { get; set; } = [];
	public IReadOnlyList<TripMembersDto> Members { get; set; } = [];
	public static TripDto ToDto(Trip t)
	{
		return new()
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
			IsExpired = t.IsExpired,
			CreatedAt = t.CreatedAt ?? DateTime.UtcNow,
			UpdatedAt = t.UpdatedAt,
			Country = t.TripCountries?.Country != null ? new CountryDto
			{
				Id = t.TripCountries.Country.Id,
				Name = t.TripCountries.Country.Name,
				Code = t.TripCountries.Country.Code
			} : new CountryDto
			{
				Id = 0,
				Name = string.Empty,
				Code = string.Empty
			},
			City = t.TripCities.Select(tc => tc.City is null ? new CityDto
			{
				Id = 0,
				Name = string.Empty,
				CountryId = 0
			}
			: new CityDto
			{
				Id = tc.City.Id,
				Name = tc.City.Name,
				CountryId = tc.City.CountryId
			}).ToList(),
			Members = t.Members.ToList() ?? [],
		};
	}
}

public sealed class TripModel(AppDbContext db)
{
	public async Task<TripDto> CreateAsync(CreateTripDto dto, int userId, CancellationToken ct = default)
	{
		var trip = new Trip()
		{
			Id = 0,
			TripName = dto.TripName,
			Description = dto.Description,
			Duration = dto.EndDate.DayNumber - dto.StartDate.DayNumber + 1,
			StartDate = dto.StartDate,
			EndDate = dto.EndDate,
			Budget = dto.Budget == 0 ? 0 : dto.Budget,
			Visibility = dto.Visibility == 0 ? TripVisibility.Public : dto.Visibility,
			CreatedBy = userId,
			IsExpired = dto.EndDate < DateOnly.FromDateTime(DateTime.UtcNow),
			CreatedAt = DateTime.UtcNow
		};

		try
		{
			db.Trips.Add(trip);
			await db.SaveChangesAsync(ct);

			db.TripCountries.Add(new TripCountry { TripId = trip.Id, CountryId = dto.Country.Id });

			await db.SaveChangesAsync(ct);

			foreach (var cityId in dto.City.Select(c => c.Id))
				db.TripCities.Add(new TripCity { TripId = trip.Id, CityId = cityId });

			await db.SaveChangesAsync(ct);

			var createdTrip = await db.Trips
				.Include(t => t.TripCountries)
					.ThenInclude(tc => tc.Country)
				.Include(t => t.TripCities)
					.ThenInclude(tc => tc.City)
				.FirstAsync(t => t.Id == trip.Id, ct);

			return Trip.ToDto(createdTrip);
		}
		catch
		{
			throw new Exception("An error occurred while creating the trip. Please try again.");
		}

	}

	public async Task<CursorPage<TripDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
	{

		var country = db.TripCountries.Include(tc => tc.Country);
		var cities = db.TripCities.Include(tc => tc.City);
		var members = db.TripMembers.Include(tm => tm.User);

		var res = await new SearchQueryBuilder<Trip>(db.Trips)
		.WithKey("id", x => x.Id)
		.AddFilters(payload.Filters, field => field.ToLowerInvariant() switch
		{
			"isexpired" => x => x.IsExpired,
			"tripname" => x => x.TripName,
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

		res.Content.ToList().ForEach(trip =>
		{
			var tripCountry = country.FirstOrDefault(tc => tc.TripId == trip.Id);
			if (tripCountry?.Country != null)
			{
				trip.Country = new CountryDto
				{
					Id = tripCountry.Country.Id,
					Name = tripCountry.Country.Name,
					Code = tripCountry.Country.Code
				};
			}
			else
			{
				trip.Country = new CountryDto
				{
					Id = 0,
					Name = string.Empty,
					Code = string.Empty
				};
			}
			var tripCities = cities.Where(tc => tc.TripId == trip.Id).ToList();
			trip.City = tripCities.Select(tc => tc.City is null ? new CityDto
			{
				Id = 0,
				Name = string.Empty,
				CountryId = 0
			}
			: new CityDto
			{
				Id = tc.City.Id,
				Name = tc.City.Name,
				CountryId = tc.City.CountryId
			}).ToList();

			var tripMembers = members.Where(tm => tm.TripId == trip.Id).ToList();
			trip.Members = [.. tripMembers.Select(tm => new TripMembersDto
			{
				Id = tm.Id,
				TripId = tm.TripId,
				UserId = tm.UserId,
				DisplayName = tm.User?.DisplayName ?? tm.User?.Username ?? string.Empty,
				Role = tm.Role
			})];
		});
		return res;
	}

	public async Task<CursorPage<TripDto>> GetTripsForUser(int userId, CancellationToken ct = default)
	{
		// Get trip ids where the user is a member
		var tripIds = await db.TripMembers
			.Where(tm => tm.UserId == userId)
			.Select(tm => tm.TripId)
			.Distinct()
			.ToListAsync(ct);

		if (!tripIds.Any())
			return new CursorPage<TripDto>(new List<TripDto>(), null, false, 0);

		// Load trips with related country and city data
		var trips = await db.Trips
			.Where(t => tripIds.Contains(t.Id))
			.Include(t => t.TripCountries)
				.ThenInclude(tc => tc.Country)
			.Include(t => t.TripCities)
				.ThenInclude(tc => tc.City)
			.ToListAsync(ct);

		// Load members for these trips (with user info)
		var members = await db.TripMembers
			.Include(tm => tm.User)
			.Where(tm => tripIds.Contains(tm.TripId))
			.ToListAsync(ct);

		var dtoList = trips.Select(t =>
		{
			var dto = Trip.ToDto(t);
			var tripMembers = members
				.Where(tm => tm.TripId == t.Id)
				.Select(tm => new TripMembersDto
				{
					Id = tm.Id,
					TripId = tm.TripId,
					UserId = tm.UserId,
					DisplayName = tm.User?.DisplayName ?? tm.User?.Username ?? string.Empty,
					Role = tm.Role,
					JoinedAt = tm.JoinedAt,
					UpdatedAt = tm.UpdatedAt
				})
				.ToList();

			dto.Members = tripMembers;
			return dto;
		}).ToList();

		return new CursorPage<TripDto>(dtoList, null, false, dtoList.Count);
	}


	public async Task<TripDto?> UpdateAsync(int userId, int id, UpdateTripDto dto, CancellationToken ct = default)
	{
		var trip =  await db.Trips
			.Include(t => t.TripCountries)
			.Include(t => t.TripCities)
			.FirstOrDefaultAsync(t => t.Id == id, ct);

		if (trip is null) return null;

		var isAdmin = await db.Set<TripMembers>().AnyAsync(tm => tm.TripId == trip.Id
			&& tm.UserId == userId
			&& tm.Role == TripMemberRole.Admin, ct);

		if (!isAdmin)
		    throw new UnauthorizedAccessException("You are not authorized to update this itinerary.");

		if (dto.TripName is not null) trip.TripName = dto.TripName;
		if (dto.Visibility != trip.Visibility) trip.Visibility = dto.Visibility;
		if (dto.Description is not null) trip.Description = dto.Description;
		if (dto.StartDate != trip.StartDate) trip.StartDate = dto.StartDate;
		if (dto.EndDate != trip.EndDate) trip.EndDate = dto.EndDate;
		var calculatedDuration = dto.EndDate.DayNumber - dto.StartDate.DayNumber + 1;
		if (calculatedDuration != trip.Duration) trip.Duration = calculatedDuration;
		if (dto.Budget is not 0) trip.Budget = dto.Budget;
		trip.IsExpired = dto.EndDate < DateOnly.FromDateTime(DateTime.UtcNow);
		trip.UpdatedAt = DateTime.UtcNow;

		try
		{
			db.Trips.Update(trip);

			if (trip.TripCountries is not null)
				db.TripCountries.Remove(trip.TripCountries);
			if (trip.TripCities.Count > 0)
				db.TripCities.RemoveRange(trip.TripCities);

			await db.SaveChangesAsync(ct);

			await db.TripCountries.AddAsync(new TripCountry { TripId = id, CountryId = dto.Country.Id }, ct);

			await db.SaveChangesAsync(ct);

			foreach (var cityId in dto.City.Select(c => c.Id))
				db.TripCities.Add(new TripCity { TripId = id, CityId = cityId });

			await db.SaveChangesAsync(ct);

			var updatedTrip = await db.Trips
				.Include(t => t.TripCountries)
					.ThenInclude(tc => tc.Country)
				.Include(t => t.TripCities)
					.ThenInclude(tc => tc.City)
				.FirstAsync(t => t.Id == id, ct);

			return Trip.ToDto(updatedTrip);
		}
		catch
		{
			throw new Exception("An error occurred while updating the trip. Please try again.");
		}
	}

	public async Task<TripDto?> GetById(int id, CancellationToken ct = default)
	{
		var members = db.TripMembers.Include(tm => tm.User);
		var trip = await db.Trips
		.Include(t => t.TripCountries)
			.ThenInclude(tc => tc.Country)
		.Include(t => t.TripCities)
			.ThenInclude(tc => tc.City)
		.FirstOrDefaultAsync(t => t.Id == id, ct);
		if (trip is null) return null;

		trip.Members = [.. members.Where(tm => tm.TripId == trip.Id).Select(tm => new TripMembersDto
		{
			Id = tm.Id,
			TripId = tm.TripId,
			UserId = tm.UserId,
			DisplayName = tm.User.DisplayName ?? tm.User.Username ?? string.Empty,
			Role = tm.Role,
			JoinedAt = tm.JoinedAt,
			UpdatedAt = tm.UpdatedAt
		})];

		return Trip.ToDto(trip);
	}

	public async Task<TripTotalPriceDto?> GetTotalPriceAsync(int tripId, CancellationToken ct = default)
	{
		var tripExists = await db.Trips.AnyAsync(t => t.Id == tripId, ct);
		if (!tripExists) return null;

		var totalPrice = await db.Itineraries
			.Where(i => i.TripId == tripId)
			.SumAsync(i => i.ExpectedPrice, ct);

		return new TripTotalPriceDto
		{
			TripId = tripId,
			TotalPrice = totalPrice
		};
	}

	public async Task<bool> DeleteAsync(int userId, int id, CancellationToken ct = default)
	{

		var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id, ct);
		if (trip is null) return false;

		var isAdmin = await db.Set<TripMembers>().AnyAsync(tm => tm.TripId == trip.Id
			&& tm.UserId == userId
			&& tm.Role == TripMemberRole.Admin, ct);

		if (!isAdmin)
		{
			throw new UnauthorizedAccessException("You are not authorized to delete this Trip.");
		}

		var rows = await db.Trips.Where(u => u.Id == id).ExecuteDeleteAsync(ct);
		return rows > 0;
	}
}
