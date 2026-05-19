using System.Security.Claims;
using Trippie.Common.Services.Authentication.Extensions;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Auth.Dtos;

namespace Trippie.Modules.Auth.Service;

public sealed class AuthService(UserService userService)
{
    public async Task<UserDto> Register(RegisterDto dto, CancellationToken ct = default)
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
            Password = dto.Password
        };

        var createdUser = await userService.CreateAsync(createUserDto, ct);

        // TODO: Return the jwt generate token;

        return createdUser;
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
}
