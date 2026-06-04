
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
public sealed class CreatedTripDto
{
	public required string TripName { get; set; }
	public string? Description { get; set; }
	public int Duration { get; set; }
	public required DateTime StartDate { get; set; }
	public required DateTime EndDate { get; set; }
	public int Budget { get; set; }
	public TripVisibility Visibility { get; set; }
	public required int CreatedBy { get; set; }
}

public sealed class TripDto
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
}

public sealed class UpdateTripDto
{
	public required int Id { get; set; }
	public required string TripName { get; set; }
	public string? Description { get; set; }
	public int Duration { get; set; }
	public required DateTime StartDate { get; set; }
	public required DateTime EndDate { get; set; }
	public int Budget { get; set; }
	public TripVisibility Visibility { get; set; }
}