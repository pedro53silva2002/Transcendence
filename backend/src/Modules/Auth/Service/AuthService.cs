using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Trippie.Common.Database;
using Trippie.Common.Services.Authentication.Extensions;
using Trippie.Common.Services.Authentication.Jwt;
using Trippie.Common.Services.Authentication.Security;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Service;

public sealed class AuthService(UserService userService, IJwtTokenService jwt, AppDbContext db, IOptions<JwtOptions> jwtOptions)
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    public async Task<AuthResponseDto> Register(RegisterDto dto, CancellationToken ct = default)
    {
        if (dto.Username is null) throw new ValidationException("username", "Username can not be empty.");
        EmailVerification(dto.Email);
        PasswordVerification(dto.Password);
    
        CreateUserDto createUserDto = new()
        {
            Email = dto.Email,
            Username = dto.Username,
            Password = dto.Password,
        };

        var createdUser = await userService.CreateAsync(createUserDto, ct);

        var response = await BuildAuthResponse(createdUser, [], ct);

        return response;
    }

    public async Task<MeDto> GetMe(ClaimsPrincipal principal, CancellationToken ct = default)
    {
        var userId = principal.GetUserId() ?? throw new UnauthorizedException("User not authenticated");
        var user = await userService.GetById(userId, ct) ?? throw new NotFoundException($"User with id {userId} not found.", userId);

        var me = new MeDto
        {
            Username = user.Username,
            DisplayName = user.DisplayName,
            Email = user.Email,
            ProfilePhotoUrl = user.ProfilePhotoUrl,
            Trips = [.. principal.GetTrips().Select(t => new TripMembershipDto
            {
                TripId = t.TripId,
                Role = t.Role
            })]
        };

        return me;
    }

    public async Task<AuthResponseDto> Login(LoginDto dto, CancellationToken ct = default)
    {
        if (dto.Username is null) throw new ValidationException("username", "Username can not be empty.");
        if (dto.Password is null) throw new ValidationException("password", "Password can not be null.");

        var user = await userService.GetByUsername(dto.Username, ct) ?? throw new UnauthorizedException("User not found.");

        string passwordHash = await userService.GetPasswordByUsername(dto.Username, ct) ?? throw new UnauthorizedException("Password not found.");
        if (!new BCryptPasswordHasher().Verify(dto.Password, passwordHash)) throw new UnauthorizedException("Invalid username or password.");

        var response = await BuildAuthResponse(user, [], ct);

        return response;
    }

    public async Task<AuthResponseDto> Refresh(RefreshTokenRequestDto dto, CancellationToken ct = default)
    {
        var stored = await db.RefreshTokens.FirstOrDefaultAsync(r => r.Token == dto.RefreshToken, ct)
            ?? throw new UnauthorizedException("Invalid refresh token");

        if (!stored.IsActive)
            throw new UnauthorizedException("Refresh token is expired or has been revoked.");

        stored.RevokedAt = DateTime.UtcNow;
        db.RefreshTokens.Update(stored);

        var user = await userService.GetById(stored.UserId, ct)
            ?? throw new UnauthorizedException("User not found");

        return await BuildAuthResponse(user, [], ct);
    }

    public async Task Logout(int userId, string token, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(token))
            jwt.InvalidateToken(token);
        await db.RefreshTokens
            .Where(r => r.UserId == userId && r.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(r => r.RevokedAt, DateTime.UtcNow), ct);
    }

    public async Task<AuthResponseDto> RegisterOrLoginViaOAuthAsync(GoogleRegisterOrLoginDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ValidationException("email", "Email is required.");
        if (string.IsNullOrWhiteSpace(dto.OAuthId))
            throw new ValidationException("oauthId", "OAuth ID is required.");
        if (string.IsNullOrWhiteSpace(dto.OAuthProvider))
            throw new ValidationException("oauthProvider", "OAuth provider is required.");

        var existingOAuthUser = await userService.GetByOAuthIdAsync(dto.OAuthProvider, dto.OAuthId, ct);
        if (existingOAuthUser is not null)
            return await BuildAuthResponse(existingOAuthUser, [], ct);

        var existingEmailUser = await userService.GetByEmail(dto.Email, ct);
        if (existingEmailUser is not null)
            return await BuildAuthResponse(existingEmailUser, [], ct);

        var baseUsername = dto.Email.Split('@')[0];
        var uniqueUsername = await userService.GenerateUniqueUsername(baseUsername, ct);

        var newUser = await userService.CreateAsync(new CreateUserDto
        {
            Email = dto.Email,
            Username = uniqueUsername,
            OAuthProvider = dto.OAuthProvider,
            OAuthId = dto.OAuthId,
            ProfilePhotoUrl = dto.ProfilePhotoUrl,
            Password = null
        }, ct);

        return await BuildAuthResponse(newUser, [], ct);
    }

    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private async Task<AuthResponseDto> BuildAuthResponse(UserDto user, IReadOnlyList<JwtTripClaim> trips, CancellationToken ct)
    {
        var (accessToken, expiresAt) = jwt.GenerateToken(new JwtUserClaims
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            DisplayName = user.DisplayName,
            Trips = trips
        });

        var refreshTokenValue = GenerateRefreshToken();
        var refreshTokenExpire = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeDays);

        db.RefreshTokens.Add(new RefreshToken
        {
            Id = 0,
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAt = refreshTokenExpire,
        });

        await db.SaveChangesAsync(ct);

        return new AuthResponseDto
        {
            Token = accessToken,
            ExpiresAt = expiresAt.UtcDateTime,
            User = user,
            RefreshToken = refreshTokenValue,
            RefreshTokenExpiresAt = refreshTokenExpire
        };
    }

    private static void EmailVerification(string Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
            throw new ValidationException("email.empty", "Email cannot be null or empty.", "Email cannot be null or empty.");
        if (!Email.Contains('@'))
            throw new ValidationException("email.format.missing_at", "Email must contain exactly one '@' symbol.", "Email must contain exactly one '@' symbol.");

        string[] emailParts = Email.Split('@');

        if (emailParts.Length != 2)
            throw new ValidationException("email.format.invalid_at", "Email must contain only one '@' symbol.", "Email must contain only one '@' symbol.");

        string localPart = emailParts[0];
        string domain = emailParts[1];

        if (string.IsNullOrWhiteSpace(localPart))
            throw new ValidationException("email.local.empty", "Email username (before '@') cannot be empty.", "Email username (before '@') cannot be empty.");
        if (!domain.Contains('.'))
            throw new ValidationException("email.domain.format", "Email domain must contain a '.' (example: domain.com).", "Email domain must contain a '.' (example: domain.com).");
        
        string[] domainParts = domain.Split('.');
        
        if (domainParts.Length < 2)
            throw new ValidationException("email.domain.invalid", "Email domain must include a valid structure like domain.com.", "Email domain must include a valid structure like domain.com.");

        string domainName = domainParts[0];
        string topLevelDomain = domainParts[^1];

        if (string.IsNullOrWhiteSpace(domainName))
            throw new ValidationException("email.domain.name.empty", "Domain name cannot be empty.", "Domain name cannot be empty.");
        if (domainName.Length < 2)
            throw new ValidationException("email.domain.name.too_short", "Domain name must be at least 2 characters long.", "Domain name must be at least 2 characters long.");
        if (string.IsNullOrWhiteSpace(topLevelDomain))
            throw new ValidationException("email.tld.empty", "Top-level domain cannot be empty.", "Top-level domain cannot be empty.");
        if (topLevelDomain.Length < 2)
            throw new ValidationException("email.tld.too_short", "Top-level domain must be at least 2 characters long.", "Top-level domain must be at least 2 characters long.");
    }

    private static void PasswordVerification(string Password)
    {

        if (string.IsNullOrWhiteSpace(Password))
            throw new ValidationException("password.empty", "Password cannot be null or empty.", "Password cannot be null or empty.");
        if (Password.Length < 8 || Password.Length > 20)
            throw new ValidationException("password.length", "Password must be between 8 and 20 characters long.", "Password must be between 8 and 20 characters long.");
        if (!Password.Any(char.IsUpper))
            throw new ValidationException("password.uppercase", "Password must contain at least one uppercase letter.", "Password must contain at least one uppercase letter.");
        if (!Password.Any(char.IsLower))
            throw new ValidationException("password.lowercase", "Password must contain at least one lowercase letter.", "Password must contain at least one lowercase letter.");
        if (!Password.Any(char.IsDigit))
            throw new ValidationException("password.digit", "Password must contain at least one digit.", "Password must contain at least one digit.");
        if (!Password.Any(ch => !char.IsLetterOrDigit(ch)))
            throw new ValidationException("password.special", "Password must contain at least one special character.", "Password must contain at least one special character.");
    }
}
