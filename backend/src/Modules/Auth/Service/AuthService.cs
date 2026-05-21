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
        if (dto.Email is null) throw new ValidationException("email", "Email can not be empty.");
        if (dto.Password is null) throw new ValidationException("password", "Password can not be null.");
        if (dto.Password.Length < 8 || dto.Password.Length > 20) throw new ValidationException("password", "Password needs to be between 8 and 20 characters.");
        if (!dto.Email.Contains('@')) throw new ValidationException("email", "Email needs to have one @.");

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
        if (dto.Email is null) throw new ValidationException("email", "Email can not be empty.");
        if (dto.Password is null) throw new ValidationException("password", "Password can not be null.");

        var user = await userService.GetByEmail(dto.Email, ct) ?? throw new UnauthorizedException("User not found.");

        string passwordHash = await userService.GetPasswordByEmail(dto.Email, ct) ?? throw new UnauthorizedException("Password not found.");
        if (!new BCryptPasswordHasher().Verify(dto.Password, passwordHash)) throw new UnauthorizedException("Invalid email or password.");

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

    internal ActionResult<AuthResponseDto> Logout(string v, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
