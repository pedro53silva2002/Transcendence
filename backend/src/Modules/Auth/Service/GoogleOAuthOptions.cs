namespace Trippie.Modules.Auth.Service;

public sealed class GoogleOAuthOptions
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string CallbackUri { get; set; }
    public required string FrontendSuccessUri { get; set; }
    public required string FrontendFailureUri { get; set; }
}
