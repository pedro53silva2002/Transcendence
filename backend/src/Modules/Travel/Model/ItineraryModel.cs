using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Travel.Model;

public sealed class Itinerary
{
    public required int Id { get; set; }
    public required int TripId { get; set; }
    public Trip? Trip { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required int ExpectedPrice { get; set; }
    public required int Day { get; set; }
    public required int CreatedBy { get; set; }
    public User? User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static ItineraryDto ToDto(Itinerary i, User? user = null) => new()
    {
        Id = i.Id,
        TripId = i.TripId,
        Title = i.Title,
        Description = i.Description,
        ExpectedPrice = i.ExpectedPrice,
        Day = i.Day,
        CreatedBy = i.CreatedBy,
        ProfilePicture = user != null ? user.ProfilePhotoUrl : null,
        CreatedAt = i.CreatedAt,
        UpdatedAt = i.UpdatedAt,
    };
}

public sealed class ItineraryModel(AppDbContext db, TripMembersModel tripMembersModel)
{
    public async Task<ItineraryDto> CreateAsync(int userId, CreateItineraryDto dto, CancellationToken ct = default)
    {
        var isMember = await tripMembersModel.IsMemberAsync(userId, dto.TripId, ct);

        if (!isMember)
            throw new UnauthorizedAccessException("You are not authorized to create an itinerary for this trip.");

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

        var user = db.Users
			.Where(u => userId == u.Id)
            .FirstOrDefault();

        db.Itineraries.Add(itinerary);
        await db.SaveChangesAsync(ct);

        return Itinerary.ToDto(itinerary, user);
    }

    public async Task<CursorPage<ItineraryDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
    {
        var res = await new SearchQueryBuilder<Itinerary>(db.Itineraries)
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
            .RunAsync(x => new ItineraryDto
            {
                Id = x.Id,
                TripId = x.TripId,
                Title = x.Title,
                Description = x.Description,
                ExpectedPrice = x.ExpectedPrice,
                Day = x.Day,
                ProfilePicture = x.User!.ProfilePhotoUrl,
                CreatedBy = x.User!.Id
            }, ct);

        return res;
    }

    public async Task<ItineraryDto?> UpdateAsync(int userId, int id, UpdateItineraryDto dto, CancellationToken ct = default)
    {
        var itinerary = await db.Itineraries.FirstOrDefaultAsync(i => i.Id == id, ct);
        if (itinerary is null) return null;

        var isAdmin = await db.Set<TripMembers>().AnyAsync(tm => tm.TripId == itinerary.TripId
            && tm.UserId == userId
            && tm.Role == TripMemberRole.Admin, ct);

        if (itinerary.CreatedBy != userId && !isAdmin)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this itinerary.");
        }

        itinerary.Title = dto.Title;
        itinerary.Description = dto.Description;
        itinerary.ExpectedPrice = dto.ExpectedPrice;
        itinerary.UpdatedAt = DateTime.UtcNow;

        db.Itineraries.Update(itinerary);
        await db.SaveChangesAsync(ct);

        return Itinerary.ToDto(itinerary);
    }

    public async Task<bool> DeleteAsync(int userId, int id, CancellationToken ct = default)
    {
        var itinerary = await db.Itineraries.FirstOrDefaultAsync(i => i.Id == id, ct);
        if (itinerary is null) return false;

        var isAdmin = await db.Set<TripMembers>().AnyAsync(tm => tm.TripId == itinerary.TripId
            && tm.UserId == userId
            && tm.Role == TripMemberRole.Admin, ct);

        if (itinerary.CreatedBy != userId && !isAdmin)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this itinerary.");
        }

        var deleted = await db.Itineraries.Where(i => i.Id == id)
            .ExecuteDeleteAsync(ct);
        return deleted > 0;
    }

    public async Task<ItineraryDto?> GetById(int id, CancellationToken ct = default)
    {
        var itinerary = await db.Itineraries
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, ct);

        return itinerary is null ? null : Itinerary.ToDto(itinerary);
    }
}
