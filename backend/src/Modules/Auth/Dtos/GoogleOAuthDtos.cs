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
