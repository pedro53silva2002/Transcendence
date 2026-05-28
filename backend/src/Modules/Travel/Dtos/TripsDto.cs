
using Trippie.Common.Services.Search.Model;

namespace Trippie.Modules.Travel.Dtos;

public enum trip_visibility
{
	Public,
	Friends,
	Private
}
public sealed class CreatedTripDto
{
	public required string TripName { get; set; }
	public string? Description { get; set; }
	public string? Destination { get; set; }
	public required DateTime StartDate { get; set; }
	public required DateTime EndDate { get; set; }
	public int Budget { get; set; }
	public trip_visibility Visibility { get; set; }
	public required int CreatedBy { get; set; }
}

public sealed class TripDto
{
	public required int Id { get; set; }
	public required string TripName { get; set; }
	public string? Description { get; set; }
	public string? Destination { get; set; }
	public required DateTime StartDate { get; set; }
	public required DateTime EndDate { get; set; }
	public int Budget { get; set; }
	public trip_visibility Visibility { get; set; }
	public required int CreatedBy { get; set; }
	public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class UpdateTripDto
{
	public required int Id { get; set; }
	public required string TripName { get; set; }
	public string? Description { get; set; }
	public string? Destination { get; set; }
	public required DateTime StartDate { get; set; }
	public required DateTime EndDate { get; set; }
	public int Budget { get; set; }
	public trip_visibility Visibility { get; set; }
}