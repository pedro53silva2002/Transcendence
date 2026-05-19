namespace Trippie.Modules.Auth.Dtos;

public sealed class RegisterDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

}


public sealed class AuthResponseDto
{
    public required UserDto User { get; set; }
    public required string Token { get; set; }
    public required DateTime ExpiresAt { get; set; }
}
