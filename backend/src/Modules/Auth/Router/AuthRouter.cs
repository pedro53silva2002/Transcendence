using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Service;

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
    public async Task<IActionResult> GoogleCallback([FromQuery] string code, [FromQuery] string state, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            return BadRequest("Missing code or state");

        var (found, codeVerifier) = oauthStateStore.TryGet(state);
        if (!found || string.IsNullOrEmpty(codeVerifier))
            return BadRequest("Invalid state or missing code verifier");

        try
        {
            var idToken = await googleOAuthService.ExchangeCodeForTokenAsync(code, codeVerifier, ct);
            var tokenPayload = googleOAuthService.DecodeToken(idToken);

            var user = await service.RegisterOrLoginViaOAuthAsync(new GoogleRegisterOrLoginDto
            {
                Email = tokenPayload.Email,
                OAuthId = tokenPayload.Sub,
                OAuthProvider = "google",
                ProfilePhotoUrl = tokenPayload.Picture
            }, ct: ct);

            var frontendSuccessUri = $"{_googleOAuthOptions.FrontendSuccessUri}?success=true";
            return Redirect(frontendSuccessUri);
        }
        catch (Exception ex)
        {
            return BadRequest($"OAuth callback failed: {ex.Message}");
        }
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
    public ActionResult<AuthResponseDto> Logout(CancellationToken ct)
        => service.Logout(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty, ct);
}
