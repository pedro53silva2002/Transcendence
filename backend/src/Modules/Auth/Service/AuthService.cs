using System.ComponentModel.DataAnnotations;
using Trippie.Common.Services.Authentication.Jwt;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Service;

public sealed class AuthService(UserService userService)
{
    public async Task<UserDto> Register(RegisterDto dto, CancellationToken ct = default)
    {
        if (dto.Username is null) throw new ValidationException($"Username can not be empty.");
        if (dto.Email is null) throw new ValidationException($"Email can not be empty.");
        if (dto.Password is null) throw new ValidationException($"Password can not be null.");
        if (dto.Password.Length < 8) throw new ValidationException($"Password needs to have at least 8 characters.");
        if (!dto.Email.Contains('@')) throw new ValidationException($"Email needs to have one @.");
        
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
}
