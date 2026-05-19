namespace Trippie.Modules.Auth.Dtos;

public sealed class RegisterDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public sealed class MeDto
{
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public List<TripMembershipDto>? Trips { get; set; }
}

public sealed class TripMembershipDto
{
    public required int TripId { get; set; }
    public required string Role { get; set; }
}
