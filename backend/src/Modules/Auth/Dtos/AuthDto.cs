namespace Trippie.Modules.Auth.Dtos;

public sealed class RegisterDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
