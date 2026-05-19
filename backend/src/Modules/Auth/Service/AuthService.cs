using System.ComponentModel.DataAnnotations;
using System.Xml;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Trippie.Common.Services.Authentication.Jwt;
using Trippie.Common.Services.Authentication.Security;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Service;

public sealed class AuthService(UserService userService, IJwtTokenService jwt)
{
	public async Task<AuthResponseDto> Register(RegisterDto dto, CancellationToken ct = default)
	{
		if (dto.Username is null) throw new ValidationException($"Username cannot be empty.");
		if (dto.Email is null) throw new ValidationException($"Email cannot be empty.");
		if (dto.Password is null) throw new ValidationException($"Password cannot be null.");
		if (dto.Password.Length < 8) throw new ValidationException($"Password needs to have at least 8 characters.");
		if (!dto.Email.Contains('@')) throw new ValidationException($"Email needs to have one @.");

		CreateUserDto createUserDto = new()
		{
			Email = dto.Email,
			Username = dto.Username,
			Password = dto.Password
		};

		var createdUser = await userService.CreateAsync(createUserDto, ct);

		var (Token, ExpiresAtUtc) = jwt.GenerateToken(new JwtUserClaims
		{
			UserId = createdUser.Id,
			Email = createdUser.Email,
			Username = createdUser.Username,
			DisplayName = createdUser.DisplayName,
			Trips = []
		});

		var authResponse = new AuthResponseDto
		{
			User = createdUser,
			Token = Token,
			ExpiresAt = ExpiresAtUtc.UtcDateTime
		};

		return authResponse;
	}

	public async Task<AuthResponseDto> Login(LoginDto dto, CancellationToken ct = default)
	{
		if (dto.Email is null) throw new ValidationException($"Email cannot be empty.");
		if (dto.Password is null) throw new ValidationException($"Password cannot be null.");
		if (dto.Password.Length < 8) throw new ValidationException($"Password needs to have at least 8 characters.");
		if (!dto.Email.Contains('@')) throw new ValidationException($"Email needs to have one @.");


		var foundUser = await userService.GetByEmail(dto.Email, ct) ?? throw new ValidationException($"Email or password is incorrect.");

		string? passwordHash = await userService.GetPasswordByEmail(foundUser.Email, ct);
		if (passwordHash is null) throw new ValidationException($"Password cannot be empty in database.");
		if (new BCryptPasswordHasher().Verify(dto.Password, passwordHash) == false)
			throw new ValidationException($"Password invalid.");
		var (Token, ExpiresAtUtc) = jwt.GenerateToken(new JwtUserClaims
		{
			UserId = foundUser.Id,
			Email = foundUser.Email,
			Username = foundUser.Username,
			DisplayName = foundUser.DisplayName,
			Trips = [] // TODO: Load trips for user and convert to JwtTripClaim when we have trips submodule
		});

		var authResponse = new AuthResponseDto
		{
			User = foundUser,
			Token = Token,
			ExpiresAt = ExpiresAtUtc.UtcDateTime
		};

		return authResponse;
	}
}
