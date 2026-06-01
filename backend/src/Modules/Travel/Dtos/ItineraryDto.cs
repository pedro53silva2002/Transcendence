namespace Trippie.Modules.Travel.Dtos;

public sealed class CreateItineraryDto
{
    public required int TripId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required int ExpectedPrice { get; set; }
    public required int Day { get; set; }
}

public sealed class ItineraryDto
{
    public required int Id { get; set; }
    public required int TripId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required int ExpectedPrice { get; set; }
    public required int Day { get; set; }
    public required int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class UpdateItineraryDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required int ExpectedPrice { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public sealed class SearchItineraryDto
{
    public int? TripId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? ExpectedPrice { get; set; }
    public int? Day { get; set; }
    public int? CreatedBy { get; set; }
}
