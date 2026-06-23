using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Modules.Travel.Model;

namespace Trippie.Modules.Travel.Service;

public sealed class ItineraryService(ItineraryModel itineraryModel)
{
    public async Task<ItineraryDto?> GetById(int id, CancellationToken ct = default)
        => await itineraryModel.GetById(id, ct);

    public async Task<ItineraryDto> CreateAsync(int userId, CreateItineraryDto dto, CancellationToken ct = default)
    {
        if (dto.Day <= 0)
            throw new ArgumentException("Day must be a positive integer.");

        ValidateItinerary(dto.Title, dto.ExpectedPrice, dto.Description);

        var itinerary = await itineraryModel.CreateAsync(userId, dto, ct);
        return itinerary;
    }

    public async Task<CursorPage<ItineraryDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
        => await itineraryModel.SearchAsync(payload, ct);

    public async Task<ItineraryDto?> UpdateAsync(int userId, int id, UpdateItineraryDto dto, CancellationToken ct = default)
    {
        ValidateItinerary(dto.Title, dto.ExpectedPrice, dto.Description);

        var itinerary = await itineraryModel.UpdateAsync(userId, id, dto, ct) ?? throw new NotFoundException($"Itinerary {id} not found.", id);
        return itinerary;
    }

    public async Task<TripTotalPriceDto?> GetTotalPriceAsync(int userId, int tripId, CancellationToken ct = default)
    {
        var isMember = await tripMembersModel.IsMemberAsync(userId, tripId, ct);
        if (!isMember)
            throw new UnauthorizedAccessException("You are not a member of this trip.");

        return await tripModel.GetTotalPriceAsync(tripId, ct);
    }

    public async Task DeleteAsync(int userId, int id, CancellationToken ct = default)
    {
        var deleted = await itineraryModel.DeleteAsync(userId, id, ct);
        if (!deleted)
            throw new NotFoundException($"Itinerary {id} not found.", id);
    }

    private static void ValidateItinerary(string? title, int expectedPrice, string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");

        if (title.Length > 20)
            throw new ArgumentException("Title cannot exceed 20 characters.");

        if (description != null && description.Length > 30)
            throw new ArgumentException("Description cannot exceed 30 characters.");

        if (expectedPrice < 0)
           throw new ArgumentException("ExpectedPrice cannot be negative.");
    }
}
