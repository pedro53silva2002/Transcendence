namespace Trippie.Modules.Auth.Dtos;

public sealed class CreateUserDto
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public string? Password { get; set; }
    public string? OAuthProvider { get; set; }
    public string? OAuthId { get; set; }
    public string? ProfilePhotoUrl { get; set; }
}

public sealed class UserDto
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public string? OAuthProvider { get; set; }
    public string? OAuthId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class UpdateUserDto
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePhotoUrl { get; set; }
}
