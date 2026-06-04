using System.Runtime.CompilerServices;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal.Postgres;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
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
	public int Duration { get; set; }
	public required DateTime StartDate { get; set; }
	public required DateTime EndDate { get; set; }
	public int Budget { get; set; }
	public TripVisibility Visibility { get; set; }
	public required int CreatedBy { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }

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
		CreatedAt = t.CreatedAt,
		UpdatedAt = t.UpdatedAt,
	};
}

public sealed class TripModel(AppDbContext db)
{
	public async Task<TripDto> CreateAsync(CreatedTripDto dto, CancellationToken ct = default)
	{
		var trip = new Trip(db)
		{
			Id = 0,
			TripName = dto.TripName,
			Description = dto.Description,
			StartDate = dto.StartDate,
			EndDate = dto.EndDate,
			Duration = (dto.EndDate - dto.StartDate).Days,
			Budget = dto.Budget,
			Visibility = dto.Visibility == 0 ? TripVisibility.Public : dto.Visibility,
			CreatedBy = dto.CreatedBy,
		};
		db.Trips.Add(trip);
		await db.SaveChangesAsync(ct);
		return Trip.ToDto(trip);
	}

	public async Task<CursorPage<TripDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
	{
		var res = await new SearchQueryBuilder<Trip>(db.Trips)
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
			.RunAsync(x => Trip.ToDto(x), ct);

		return res;
	}

	public async Task<TripDto?> UpdateAsync(int userId, int id, UpdateTripDto dto, CancellationToken ct = default)
	{
		var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id, ct);
		if (trip is null) return null;
		/*if (db.Set<TripMember>().AnyAsync(tm.Role != MemberRole.Admin))
			throw new UnauthorizedException($"User Unauthorized '{userId}'.");*/

		if (dto.TripName is not null) trip.TripName = dto.TripName;
		if (dto.Visibility != trip.Visibility) trip.Visibility = dto.Visibility;
		if (dto.Description is not null) trip.Description = dto.Description;
		if (dto.StartDate != trip.StartDate) trip.StartDate = dto.StartDate;
		if (dto.EndDate != trip.EndDate) trip.EndDate = dto.EndDate;
		if (dto.Duration != trip.Duration) trip.Duration = dto.Duration;
		if (dto.Budget is not 0) trip.Budget = dto.Budget;
		trip.UpdatedAt = DateTime.UtcNow;

		db.Trips.Update(trip);
		await db.SaveChangesAsync(ct);

		return Trip.ToDto(trip);
	}

	public async Task<TripDto?> GetById(int id, CancellationToken ct = default)
	{
		var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id, ct);
		if (trip is not null)
			return Trip.ToDto(trip);
		return null;
	}

	public async Task<bool> DeleteAsync(int userId, int id, CancellationToken ct = default)
	{
		var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id, ct);
		if (trip is null) return false;
		/*if (db.Set<TripMember>().AnyAsync(tm.Role != MemberRole.Admin))
			throw new UnauthorizedException($"User Unauthorized '{userId}'.");*/
		var rows = await db.Trips.Where(u => u.Id == id).ExecuteDeleteAsync(ct);
		return rows > 0;
	}
}
