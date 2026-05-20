namespace Trippie.Modules.Auth.Dtos;

public sealed class GoogleOAuthCallbackDto
{
    public required string Code { get; set; }
    public required string State { get; set; }
}

public sealed class GoogleAuthUrlDto
{
    public required string AuthorizationUrl { get; set; }
    public required string State { get; set; }
    public required string CodeVerifier { get; set; }
}

public sealed class GoogleRegisterOrLoginDto
{
    public required string Email { get; set; }
    public required string OAuthId { get; set; }
    public required string OAuthProvider { get; set; }
    public string? ProfilePhotoUrl { get; set; }
}
