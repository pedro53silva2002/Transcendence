//using Trippie.Common.Services.Authentication.Jwt;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Auth.Dtos;

namespace Trippie.Modules.Auth.Service;

public sealed class AuthService(UserService userService)
{
    public async Task<UserDto> Register(RegisterDto dto, CancellationToken ct = default)
    {
        if (dto.Username is null)
            throw new ValidationException("username", "Username can not be empty.");
        if (dto.Email is null)
            throw new ValidationException("email", "Email can not be empty.");
        if (dto.Password is null)
            throw new ValidationException("password", "Password can not be null.");
        if (dto.Password.Length < 8)
            throw new ValidationException("password", "Password needs to have at least 8 characters.");
        if (!dto.Email.Contains('@'))
            throw new ValidationException("email", "Email needs to have one @.");

        CreateUserDto createUserDto = new()
        {
            Email = dto.Email,
            Username = dto.Username,
            Password = dto.Password
        };

        var createdUser = await userService.CreateAsync(createUserDto, ct);

        // TODO: Return the jwt generate token - not now;

        return createdUser;
    }

    public async Task<UserDto> RegisterOrLoginViaOAuthAsync(string email, string oauthId, string oauthProvider, string? profilePhotoUrl, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("email", "Email is required.");
        if (string.IsNullOrWhiteSpace(oauthId))
            throw new ValidationException("oauthId", "OAuth ID is required.");
        if (string.IsNullOrWhiteSpace(oauthProvider))
            throw new ValidationException("oauthProvider", "OAuth provider is required.");

        var existingOAuthUser = await userService.GetByOAuthIdAsync(oauthProvider, oauthId, ct);
        if (existingOAuthUser is not null)
            return existingOAuthUser;

        var existingEmailUser = await userService.GetByEmail(email, ct);
        if (existingEmailUser is not null)
        {
            if (existingEmailUser.OAuthProvider is null)
                throw new ConflictException($"User with this email already exists with traditional signup.");
            throw new ConflictException($"User with this email already exists.");
        }

        var baseUsername = email.Split('@')[0];
        var uniqueUsername = await userService.GenerateUniqueUsername(baseUsername, ct);

        var newUser = await userService.CreateOAuthAsync(
            email: email,
            username: uniqueUsername,
            displayName: uniqueUsername,
            oauthProvider: oauthProvider,
            oauthId: oauthId,
            profilePhotoUrl: profilePhotoUrl,
            ct: ct
        );

        return newUser;
    }
}
