using System.Text.Json.Serialization;
using NpgsqlTypes;
using Trippie.Common.Services.Search.Model;

namespace Trippie.Modules.Travel.Dtos;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TripVisibility
{
	[PgName("public")] Public,
	[PgName("friends")] Friends,
	[PgName("private")] Private
}

public sealed class CreateTripDto
{
	public required string TripName { get; set; }
	public string? Description { get; set; }
	public required DateTime StartDate { get; set; }
	public required DateTime EndDate { get; set; }
	public required int Budget { get; set; }
	public required TripVisibility Visibility { get; set; }
	public required int CreatedBy { get; set; }
	public required int CountryId { get; set; }
	public List<int> CityIds { get; set; } = [];	 
}

public sealed class TripDto
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
	public required DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public required int CountryId { get; set; }	
	public List<int> CityIds { get; set; } = [];	
}

public sealed class UpdateTripDto
{
	public required int Id { get; set; }
	public required string TripName { get; set; }
	public string? Description { get; set; }
	public required DateTime StartDate { get; set; }
	public required DateTime EndDate { get; set; }
	public required int Budget { get; set; }
	public required TripVisibility Visibility { get; set; }
	public required int CountryId { get; set; }	
	public List<int> CityIds { get; set; } = [];
}
