using System.Text.Json.Serialization;
using NpgsqlTypes;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;

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
	public required DateOnly StartDate { get; set; }
	public required DateOnly EndDate { get; set; }
	public required int Budget { get; set; }
	public required TripVisibility Visibility { get; set; }
	public required CountryDto Country { get; set; }
	public List<CityDto> City { get; set; } = [];
	public required CreateTripMembersDto Members { get; set; }
}

public sealed class TripDto
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
	public required DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public required CountryDto Country { get; set; }
	public List<CityDto> City { get; set; } = [];
	public List<TripMembersDto>? Members { get; set; }
	public required bool IsExpired { get; set; }
}

public sealed class UpdateTripDto
{
	public required int Id { get; set; }
	public required string TripName { get; set; }
	public string? Description { get; set; }
	public required DateOnly StartDate { get; set; }
	public required DateOnly EndDate { get; set; }
	public required int Budget { get; set; }
	public required TripVisibility Visibility { get; set; }
	public required CountryDto Country { get; set; }
	public List<CityDto> City { get; set; } = [];
	public required bool IsExpired { get; set; }
}

public sealed class TripTotalPriceDto
{
    public required int TripId { get; set; }
    public required int TotalPrice { get; set; }
}
