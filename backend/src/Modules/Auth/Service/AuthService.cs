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
}
