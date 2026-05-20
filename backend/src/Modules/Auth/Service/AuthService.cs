using System.Security.Claims;
using Trippie.Common.Services.Authentication.Extensions;
using Trippie.Common.Services.Authentication.Jwt;
using Trippie.Common.Services.Authentication.Security;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Auth.Dtos;

namespace Trippie.Modules.Auth.Service;

public sealed class AuthService(UserService userService, IJwtTokenService jwt)
{
    public async Task<AuthResponseDto> Register(RegisterDto dto, CancellationToken ct = default)
    {
        if (dto.Username is null) throw new ValidationException("username", "Username can not be empty.");
        if (dto.Email is null) throw new ValidationException("email", "Email can not be empty.");
        if (dto.Password is null) throw new ValidationException("password", "Password can not be null.");
        if (dto.Password.Length < 8) throw new ValidationException("password", "Password needs to have at least 8 characters.");
        if (!dto.Email.Contains('@')) throw new ValidationException("email", "Email needs to have one @.");

        CreateUserDto createUserDto = new()
        {
            Email = dto.Email,
            Username = dto.Username,
            Password = dto.Password,
        };

        var createdUser = await userService.CreateAsync(createUserDto, ct);

        var (token, expiresAt) = jwt.GenerateToken(new JwtUserClaims
        {
            UserId = createdUser.Id,
            Username = createdUser.Username,
            Email = createdUser.Email,
            DisplayName = createdUser.DisplayName,
            Trips = []
        });

        var response = new AuthResponseDto
        {
            Token = token,
            User = createdUser,
            ExpiresAt = expiresAt.UtcDateTime
        };

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

        var (token, expiresAt) = jwt.GenerateToken(new JwtUserClaims
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            DisplayName = user.DisplayName,
            Trips = []
        });

        var response = new AuthResponseDto
        {
            Token = token,
            User = user,
            ExpiresAt = expiresAt.UtcDateTime
        };

        return response;
    }

    public async Task<AuthResponseDto> Logout(string token, CancellationToken ct = default)
    {
        jwt.InvalidateToken(token);
        return new AuthResponseDto
        {
            Token = string.Empty,
            User = null!,
            ExpiresAt = DateTime.UtcNow
        };
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
        {
            var (tokenUser, expiresAtUser) = jwt.GenerateToken(new JwtUserClaims
            {
                UserId = existingOAuthUser.Id,
                Email = existingOAuthUser.Email,
                Username = existingOAuthUser.Username,
                DisplayName = existingOAuthUser.DisplayName,
                Trips = [] // TODO: Map trips to JwtTripClaim
            });

            var responseDtoUser = new AuthResponseDto
            {
                User = existingOAuthUser,
                Token = tokenUser,
                ExpiresAt = expiresAtUser.UtcDateTime
            };

            return responseDtoUser;
        }

        var existingEmailUser = await userService.GetByEmail(dto.Email, ct);
        if (existingEmailUser is not null)
        {
            if (existingEmailUser.OAuthProvider == "none")
                throw new ConflictException($"User with this email already exists with traditional signup.");

            var (tokenEmail, expiresAtEmail) = jwt.GenerateToken(new JwtUserClaims
            {
                UserId = existingEmailUser.Id,
                Email = existingEmailUser.Email,
                Username = existingEmailUser.Username,
                DisplayName = existingEmailUser.DisplayName,
                Trips = [] // TODO: Map trips to JwtTripClaim
            });

            var responseDtoEmail = new AuthResponseDto
            {
                User = existingEmailUser,
                Token = tokenEmail,
                ExpiresAt = expiresAtEmail.UtcDateTime
            };

            return responseDtoEmail;
        }

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

        var (token, expiresAt) = jwt.GenerateToken(new JwtUserClaims
        {
            UserId = newUser.Id,
            Email = newUser.Email,
            Username = newUser.Username,
            DisplayName = newUser.DisplayName,
            Trips = [] // TODO: Map trips to JwtTripClaim
        });

        var responseDto = new AuthResponseDto
        {
            User = newUser,
            Token = token,
            ExpiresAt = expiresAt.UtcDateTime
        };

        return responseDto;
    }
}
