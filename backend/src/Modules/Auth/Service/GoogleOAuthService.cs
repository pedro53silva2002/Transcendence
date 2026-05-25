using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Trippie.Modules.Auth.Service;

public sealed class GoogleOAuthService(HttpClient httpClient, IOptions<GoogleOAuthOptions> options)
{
    private readonly GoogleOAuthOptions _options = options.Value;
    private const string GoogleTokenEndpoint = "https://oauth2.googleapis.com/token";
    private const string GoogleUserInfoEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";

    public (string CodeVerifier, string CodeChallenge, string State) GeneratePkceAndState()
    {
        var codeVerifier = GenerateRandomString(32);
        var codeChallenge = GenerateCodeChallenge(codeVerifier);
        var state = GenerateRandomString(32);
        return (codeVerifier, codeChallenge, state);
    }

    public string GetAuthorizationUrl(string state, string codeChallenge)
    {
        var scope = "openid email profile";
        var prompt = "consent";

        return $"https://accounts.google.com/o/oauth2/v2/auth?" +
            $"client_id={Uri.EscapeDataString(_options.ClientId)}&" +
            $"redirect_uri={Uri.EscapeDataString(_options.CallbackUri)}&" +
            $"response_type=code&" +
            $"scope={Uri.EscapeDataString(scope)}&" +
            $"code_challenge={Uri.EscapeDataString(codeChallenge)}&" +
            $"code_challenge_method=S256&" +
            $"state={Uri.EscapeDataString(state)}&" +
            $"prompt={prompt}";
    }

    public async Task<string> ExchangeCodeForTokenAsync(string code, string codeVerifier, CancellationToken ct = default)
    {
        var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code", code },
            { "client_id", _options.ClientId },
            { "client_secret", _options.ClientSecret },
            { "redirect_uri", _options.CallbackUri },
            { "code_verifier", codeVerifier }
        });

        var response = await httpClient.PostAsync(GoogleTokenEndpoint, requestContent, ct);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(content);
        var idToken = doc.RootElement.GetProperty("id_token").GetString();

        return idToken ?? throw new InvalidOperationException("No id_token in response");
    }

    public GoogleTokenPayload DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        var name = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        var picture = jwtToken.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
        var sub = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sub))
            throw new InvalidOperationException("Invalid token: missing email or sub claim");

        return new GoogleTokenPayload
        {
            Email = email,
            Name = name ?? email,
            Picture = picture,
            Sub = sub
        };
    }

    private static string GenerateRandomString(int length)
    {
        var buffer = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(buffer);
        }
        return Convert.ToBase64String(buffer).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    private static string GenerateCodeChallenge(string codeVerifier)
    {
        var bytes = Encoding.UTF8.GetBytes(codeVerifier);
        using (var sha256 = SHA256.Create())
        {
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}

public sealed class GoogleTokenPayload
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public string? Picture { get; set; }
    public required string Sub { get; set; }
}
