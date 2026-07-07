using System.Text.Json.Serialization;
using NpgsqlTypes;

namespace Trippie.Modules.Travel.Dtos;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TripMemberRole
{
	[PgName("admin")] Admin,
	[PgName("member")] Member,
}

public sealed class CreateTripMembersDto
{
    public required int TripId { get; set; }
    public required IReadOnlyList<int> UserIds { get; set; }
}

public sealed class TripMembersDto
{
    public required int Id { get; set; }
    public required int TripId { get; set; }
    public required int UserId { get; set; }
	public required string DisplayName { get; set; }
    public required string Username { get; set; }
	public string? ProfilePicture { get; set; }
	public required TripMemberRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class UpdateTripMembersDto
{
    public required int UserId { get; set; }
    public required TripMemberRole Role { get; set; }
}
