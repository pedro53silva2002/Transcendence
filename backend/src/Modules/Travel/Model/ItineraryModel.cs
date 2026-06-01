using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;

namespace Trippie.Modules.Travel.Model;

public sealed class Itinerary
{
    public required int Id { get; set; }
    public required int TripId { get; set; }
    //public Trip? Trip { get; set; } // Navigation to Trip (many-to-one). To uncomment after Coletes finish this implementation
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required int ExpectedPrice { get; set; }
    public required int Day { get; set; }
    public required int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static ItineraryDto ToDto(Itinerary i) => new()
    {
        Id = i.Id,
        TripId = i.TripId,
        Title = i.Title,
        Description = i.Description,
        ExpectedPrice = i.ExpectedPrice,
        Day = i.Day,
        CreatedBy = i.CreatedBy,
        CreatedAt = i.CreatedAt,
        UpdatedAt = i.UpdatedAt,
    };
}

public sealed class ItineraryModel(AppDbContext db)
{
    public async Task<ItineraryDto> CreateAsync(int userId, CreateItineraryDto dto, CancellationToken ct = default)
    {
        /* var isMember = await db.Set<TripMember>().AnyAsync(tm => tm.TripId == dto.TripId && tm.UserId == userId, ct);

        if (!isMember)
            throw new UnauthorizedAccessException("You are not authorized to create an itinerary for this trip."); */

        var itinerary = new Itinerary
        {
			Id = 0,
            TripId = dto.TripId,
            Title = dto.Title,
            Description = dto.Description,
        	ExpectedPrice = dto.ExpectedPrice,
        	Day = dto.Day,
        	CreatedBy = userId,
    	};

    	db.Itinerary.Add(itinerary);
		await db.SaveChangesAsync(ct);

	    return Itinerary.ToDto(itinerary);
	}

    public async Task<CursorPage<ItineraryDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
    {
        var res = await new SearchQueryBuilder<Itinerary>(db.Itinerary)
            .WithKey("id", x => x.Id)
            .AddFilters(payload.Filters, field => field.ToLowerInvariant() switch
            {
                "id" => x => x.Id,
                "tripid" => x => x.TripId,
                "title" => x => x.Title,
                "expectedprice" => x => x.ExpectedPrice,
                "day" => x => x.Day,
                "createdby" => x => x.CreatedBy,
                _ => throw new SearchValidationException($"Invalid filter field '{field}'."),
            })
            .SetOrderBy(payload.Sort, field => field.ToLowerInvariant() switch
            {
                "id" => x => x.Id,
                "tripid" => x => x.TripId,
                "title" => x => x.Title,
                "expectedprice" => x => x.ExpectedPrice,
                "day" => x => x.Day,
                "createdby" => x => x.CreatedBy,
                _ => throw new SearchValidationException($"Invalid sort field '{field}'."),
            })
            .SetCursorPagination(payload.Page)
            .RunAsync(x => Itinerary.ToDto(x), ct);

        return res;
    }

    public async Task<ItineraryDto?> UpdateAsync(int userId, int id, UpdateItineraryDto dto, CancellationToken ct = default)
    {
        var itinerary = await db.Itinerary.FirstOrDefaultAsync(i => i.Id == id, ct);
        if (itinerary is null) return null;

        if (itinerary.CreatedBy != userId /* || db.Set<TripMember>().AnyAsync(tm => tm.TripId == itinerary.TripId
            && tm.UserId == userId
            && tm.Role != MemberRole.Admin) */)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this itinerary.");
        }

        itinerary.Title = dto.Title;
        itinerary.Description = dto.Description;
        itinerary.ExpectedPrice = dto.ExpectedPrice;
        itinerary.UpdatedAt = DateTime.UtcNow;

        db.Itinerary.Update(itinerary);
        await db.SaveChangesAsync(ct);

        return Itinerary.ToDto(itinerary);
    }

    public async Task<bool> DeleteAsync(int userId, int id, CancellationToken ct = default)
    {
        var deleted = await db.Itinerary.Where(i => i.Id == id
            && (
                i.CreatedBy == userId /* ||
                db.Set<TripMember>().AnyAsync(tm => tm.TripId == i.TripId
                    && tm.UserId == userId
                    && tm.Role == MemberRole.Admin)
            */))
            .ExecuteDeleteAsync(ct);
        return deleted > 0;
    }

    public async Task<ItineraryDto?> GetById(int id, CancellationToken ct = default)
    {
        var itinerary = await db.Itinerary
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, ct);

        return itinerary is null ? null : Itinerary.ToDto(itinerary);
    }
}
