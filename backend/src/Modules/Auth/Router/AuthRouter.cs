using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trippie.Common.Services.Authentication.Extensions;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Service;
using Microsoft.AspNetCore.RateLimiting;

namespace Trippie.Modules.Auth.Router;

[ApiController]
[Route("api/auth")]
public sealed class AuthRouter(AuthService service, GoogleOAuthService googleOAuthService, IOptions<GoogleOAuthOptions> googleOAuthOptions, OAuthStateStore oauthStateStore) : ControllerBase
{
    private readonly GoogleOAuthOptions _googleOAuthOptions = googleOAuthOptions.Value;

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto, CancellationToken ct)
    {
        return await service.Register(dto, ct);
    }

    [HttpGet("google/url")]
    public ActionResult<GoogleAuthUrlDto> GetGoogleAuthUrl()
    {
        var (codeVerifier, codeChallenge, state) = googleOAuthService.GeneratePkceAndState();
        oauthStateStore.Store(state, codeVerifier);

        var authUrl = googleOAuthService.GetAuthorizationUrl(state, codeChallenge);

        return Ok(new GoogleAuthUrlDto
        {
            AuthorizationUrl = authUrl,
            State = state,
            CodeVerifier = codeVerifier
        });
    }

    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback(
        [FromQuery] string? code,
        [FromQuery] string? state,
        [FromQuery] string? error,
        [FromQuery(Name = "error_description")] string? errorDescription,
        CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(error))
            return Redirect(BuildFailureRedirectUri(error, errorDescription));

        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state))
            return Redirect(BuildFailureRedirectUri("missing_code_or_state"));

        var (found, codeVerifier) = oauthStateStore.TryGet(state);
        if (!found || string.IsNullOrEmpty(codeVerifier))
            return Redirect(BuildFailureRedirectUri("invalid_state_or_missing_code_verifier"));

        try
        {
            var idToken = await googleOAuthService.ExchangeCodeForTokenAsync(code, codeVerifier, ct);
            var tokenPayload = googleOAuthService.DecodeToken(idToken);

            var authResponse = await service.RegisterOrLoginViaOAuthAsync(new GoogleRegisterOrLoginDto
            {
                Email = tokenPayload.Email,
                OAuthId = tokenPayload.Sub,
                OAuthProvider = "google",
                ProfilePhotoUrl = tokenPayload.Picture
            }, ct: ct);

            var frontendUri = $"{_googleOAuthOptions.FrontendUri}#token={Uri.EscapeDataString(authResponse.Token)}&refreshToken={Uri.EscapeDataString(authResponse.RefreshToken)}";
            return Redirect(frontendUri);
        }
        catch
        {
            return Redirect(BuildFailureRedirectUri("oauth_callback_failed"));
        }
    }

    private string BuildFailureRedirectUri(string reason, string? message = null)
    {
        var separator = _googleOAuthOptions.FrontendUri.Contains('?') ? "&" : "?";
        var redirectUri = $"{_googleOAuthOptions.FrontendUri}{separator}error={Uri.EscapeDataString(reason)}";

        if (!string.IsNullOrWhiteSpace(message))
            redirectUri += $"&message={Uri.EscapeDataString(message)}";

        return redirectUri;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<MeDto>> Me(CancellationToken ct)
    => await service.GetMe(User, ct);

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto, CancellationToken ct)
        => await service.Login(dto, ct);

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        {
            var userId = User.GetUserId()
                ?? throw new UnauthorizedException("User not authenticated.");

            var rawToken = Request.Headers.Authorization
                .FirstOrDefault()
                ?.Replace("Bearer ", string.Empty)
                ?? string.Empty;

            await service.Logout(userId, rawToken, ct);
            return NoContent();
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenRequestDto dto, CancellationToken ct)
        => await service.Refresh(dto, ct);

}
