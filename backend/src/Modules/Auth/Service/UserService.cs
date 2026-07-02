using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Model;
using System.Security.Cryptography;
using System.Text;

namespace Trippie.Modules.Auth.Service;

public sealed class UserService(AppDbContext db, UserModel userModel)
{
    private const int UsernameMaxLength = 15;
    private const int SuffixLength = 6;

    public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Email)) throw new ValidationException("email", "Email is required.");
        if (string.IsNullOrWhiteSpace(dto.Username)) throw new ValidationException("username", "Username is required.");
		if (dto.Username.Length > UsernameMaxLength) throw new ValidationException("username", $"Username cannot exceed {UsernameMaxLength} characters.");

        var existsUsername = await userModel.GetByUsername(dto.Username, ct);
        var existsEmails = await userModel.GetByEmail(dto.Email, ct);

        if (existsEmails is not null || existsUsername is not null)
        {
            var field = existsEmails is not null ? "email" : "username";
            throw new ConflictException($"User with this {field} already exists.");
        }

        var user = await userModel.CreateAsync(dto, ct);
        return user;
    }

    public async Task<CursorPage<UserDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
        => await userModel.SearchAsync(payload, ct);

    public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default)
    {
        if (dto.Email is not null && string.IsNullOrWhiteSpace(dto.Email))
            throw new ValidationException("email", "Email can not be empty.");
        if (dto.Username is not null && string.IsNullOrWhiteSpace(dto.Username))
            throw new ValidationException("username", "Username can not be blank.");
		if (dto.Username is not null && dto.Username.Length > UsernameMaxLength)
			throw new ValidationException("username", $"Username cannot exceed {UsernameMaxLength} characters.");
		if (dto.DisplayName is not null && dto.DisplayName.Length > UsernameMaxLength)
			throw new ValidationException("displayName", $"Display name cannot exceed {UsernameMaxLength} characters.");

        if (dto.Email is not null || dto.Username is not null)
        {
            var clash = await db.Users
                .AsNoTracking()
                .Where(u => u.Id != id
                    && ((dto.Email != null && u.Email == dto.Email)
                        || (dto.Username != null && u.Username == dto.Username)))
                .FirstOrDefaultAsync(ct);
            if (clash is not null)
                throw new ConflictException("Email or username already in use.");
        }

        var user = await userModel.UpdateAsync(id, dto, ct) ?? throw new NotFoundException($"User {id} not found.", id);
    
        return user;
    }

	public async Task UpdateProfilePhotoAsync(int id, string? profilePhotoUrl, CancellationToken ct = default)
    	=> await userModel.UpdateProfilePhotoAsync(id, profilePhotoUrl, ct);

    public async Task<UserDto?> GetByEmail(string email, CancellationToken ct = default)
    {
        if (email is null) throw new ValidationException("email", $"Email cannot be empty.");

        var res = await userModel.GetByEmail(email, ct);

        return res;
    }

	 public async Task<UserDto?> GetByUsername(string username, CancellationToken ct = default)
    {
        if (username is null) throw new ValidationException("username", $"Username cannot be empty.");

        var res = await userModel.GetByUsername(username, ct);

        return res;
    }

    public async Task<UserDto?> GetById(int id, CancellationToken ct = default)
    {
        var res = await userModel.GetById(id, ct);
        if (res is null)
            return null;
        return res;
    }
    public string GenerateUniqueUsername(string baseUsername)
    {
        if (string.IsNullOrWhiteSpace(baseUsername))
            throw new ValidationException("baseUsername", "Base username is required.");

        var prefix = NormalizeUsername(baseUsername);
        var maxPrefixLength = UsernameMaxLength - SuffixLength - 1;

        if (prefix.Length > maxPrefixLength)
            prefix = prefix[..maxPrefixLength];

        var suffix = GenerateRandomSuffix(SuffixLength);
        var username = $"{prefix}_{suffix}";
        
        return username;
    }

    private static string NormalizeUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ValidationException("username", "Username is required.");

        var sb = new StringBuilder();

        foreach (var c in username.ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(c))
                sb.Append(c);
        }

        return sb.Length == 0 ? "user" : sb.ToString();
    }

    public static string GenerateRandomSuffix(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        
        char[] buffer = new char[length];

        for (int i = 0; i < length; i++)
            buffer[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];

        return new string(buffer);
    }

    public async Task<UserDto?> GetByOAuthIdAsync(string oauthProvider, string oauthId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(oauthId))
            throw new ValidationException("oauthId", "OAuth ID is required.");

        return await userModel.GetByOAuthIdAsync(oauthProvider, oauthId, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var deleted = await userModel.DeleteAsync(id, ct);
        if (!deleted) throw new NotFoundException($"User {id} not found.", id);
    }

    public async Task<string?> GetPasswordByUsername(string username, CancellationToken ct = default)
    {
        if (username is null) throw new ValidationException("email", $"Email cannot be empty");

        var res = await userModel.GetPasswordByUsername(username, ct);
        if (res is not null)
            return res;
        return null;
    }
}
