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
    public int Duration { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public int Budget { get; set; }
    public TripVisibility Visibility { get; set; }
    public required int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static TripDto ToDto(Trip t, int countryId, List<int> cityIds) => new()
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
        CountryId = countryId,   // added
        CityIds = cityIds,        // added
    };
}

public sealed class TripModel(AppDbContext db)
{
    public async Task<TripDto> CreateAsync(CreatedTripDto dto, CancellationToken ct = default)
    {
        await using var tx = await db.Database.BeginTransactionAsync(ct);

        var trip = new Trip(db)
        {
            Id = 0,
            TripName = dto.TripName,
            Description = dto.Description,
            Duration = dto.Duration,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Budget = dto.Budget,
            Visibility = dto.Visibility == 0 ? TripVisibility.Public : dto.Visibility,
            CreatedBy = dto.CreatedBy,
        };

        db.Trips.Add(trip);
        await db.SaveChangesAsync(ct);

        db.TripCountries.Add(new TripCountry { TripId = trip.Id, CountryId = dto.CountryId });

        foreach (var cityId in dto.CityIds)
            db.TripCities.Add(new TripCity { TripId = trip.Id, CityId = cityId });

        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return Trip.ToDto(trip, dto.CountryId, dto.CityIds);
    }

    public async Task<CursorPage<TripDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
    {
        var res = await new SearchQueryBuilder<Trip>(db.Trips)
            .WithKey("id", x => x.Id)
            .AddFilters(payload.Filters, field => field.ToLowerInvariant() switch
            {
                "tripname"    => x => x.TripName,
                "startdate"   => x => x.StartDate,
                "createdat"   => x => x.CreatedAt,
                "id"          => x => x.Id,
                "visibility"  => x => x.Visibility,
                _ => throw new SearchValidationException($"Unknown filter field '{field}'."),
            })
            .SetOrderBy(payload.Sort, field => field.ToLowerInvariant() switch
            {
                "tripname"    => x => x.TripName,
                "duration"    => x => x.Duration,
                "startdate"   => x => x.StartDate,
                "createdat"   => x => x.CreatedAt,
                "id"          => x => x.Id,
                _ => throw new SearchValidationException($"Unsortable field '{field}'."),
            })

            .SetCursorPagination(payload.Page)
            .RunAsync(x => Trip.ToDto(x, 0, new List<int>()), ct);  // join data not loaded in search results

        return res;
    }

    public async Task<TripDto?> UpdateAsync(int userId, int id, UpdateTripDto dto, CancellationToken ct = default)
    {
        await using var tx = await db.Database.BeginTransactionAsync(ct);

        var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (trip is null) return null;

        if (dto.TripName is not null) trip.TripName = dto.TripName;
        if (dto.Visibility != trip.Visibility) trip.Visibility = dto.Visibility;
        if (dto.Description is not null) trip.Description = dto.Description;
        if (dto.StartDate != trip.StartDate) trip.StartDate = dto.StartDate;
        if (dto.EndDate != trip.EndDate) trip.EndDate = dto.EndDate;
        if (dto.Duration != trip.Duration) trip.Duration = dto.Duration;
        if (dto.Budget is not 0) trip.Budget = dto.Budget;
        trip.UpdatedAt = DateTime.UtcNow;

        db.Trips.Update(trip);

        // Full replace of join records
        await db.TripCountries.Where(tc => tc.TripId == id).ExecuteDeleteAsync(ct);
        await db.TripCities.Where(tc => tc.TripId == id).ExecuteDeleteAsync(ct);

        db.TripCountries.Add(new TripCountry { TripId = id, CountryId = dto.CountryId });

        foreach (var cityId in dto.CityIds)
            db.TripCities.Add(new TripCity { TripId = id, CityId = cityId });

        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return Trip.ToDto(trip, dto.CountryId, dto.CityIds);
    }

    public async Task<TripDto?> GetById(int id, CancellationToken ct = default)
    {
        var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (trip is null) return null;

        var countryId = await db.TripCountries
            .Where(tc => tc.TripId == id)
            .Select(tc => tc.CountryId)
            .FirstOrDefaultAsync(ct);

			var cityIds = await db.TripCities
            .Where(tc => tc.TripId == id)
            .Select(tc => tc.CityId)
            .ToListAsync(ct);
			if (cityIds is null) cityIds = new List<int>();

        return Trip.ToDto(trip, countryId, cityIds);
    }

    public async Task<bool> DeleteAsync(int userId, int id, CancellationToken ct = default)
    {
        var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (trip is null) return false;

        // TripCountries and TripCities are cleaned up automatically via ON DELETE CASCADE
        var rows = await db.Trips.Where(u => u.Id == id).ExecuteDeleteAsync(ct);
        return rows > 0;
    }
}