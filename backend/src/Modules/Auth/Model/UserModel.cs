using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.Authentication.Security;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;

namespace Trippie.Modules.Auth.Model;

public sealed class User
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public string? PasswordHash { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public required string OauthProvider { get; set; }
    public string? OauthId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public static UserDto ToDto(User u) => new()
    {
        Id = u.Id,
        Email = u.Email,
        Username = u.Username,
        DisplayName = u.DisplayName,
        Bio = u.Bio,
        ProfilePhotoUrl = u.ProfilePhotoUrl,
        OAuthProvider = u.OauthProvider,
        OAuthId = u.OauthId,
        CreatedAt = u.CreatedAt,
        UpdatedAt = u.UpdatedAt,
    };
}

public sealed class UserModel(AppDbContext db)
{
    public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        string? passwordHash = null;
        if (!string.IsNullOrEmpty(dto.Password))
            passwordHash = new BCryptPasswordHasher().Hash(dto.Password);

        var user = new User
        {
            Id = 0,
            Email = dto.Email,
            Username = dto.Username,
            DisplayName = dto.Username,
            PasswordHash = passwordHash,
            OauthProvider = dto.OAuthProvider ?? "none",
        };

        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return User.ToDto(user);
    }

    public async Task<CursorPage<UserDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
    {
        var res = await new SearchQueryBuilder<User>(db.Users)
            .WithKey("id", x => x.Id)
            .AddFilters(payload.Filters, field => field.ToLowerInvariant() switch
            {
                "email" => x => x.Email,
                "username" => x => x.Username,
                "displayname" => x => x.DisplayName,
                "createdat" => x => x.CreatedAt,
                "id" => x => x.Id,
                _ => throw new SearchValidationException($"Unknown filter field '{field}'."),
            })
            .SetOrderBy(payload.Sort, field => field.ToLowerInvariant() switch
            {
                "username" => x => x.Username,
                "displayname" => x => x.DisplayName,
                "createdat" => x => x.CreatedAt,
                "id" => x => x.Id,
                _ => throw new SearchValidationException($"Unsortable field '{field}'."),
            })
            .SetCursorPagination(payload.Page)
            .RunAsync(x => User.ToDto(x), ct);

        return res;
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return null;

        if (dto.Email is not null) user.Email = dto.Email;
        if (dto.Username is not null) user.Username = dto.Username;
        if (dto.DisplayName is not null) user.DisplayName = dto.DisplayName;
        user.ProfilePhotoUrl = dto.ProfilePhotoUrl;
        user.Bio = dto.Bio;
        user.UpdatedAt = DateTime.UtcNow;

        db.Users.Update(user);
        await db.SaveChangesAsync(ct);

        return User.ToDto(user);
    }

    public async Task<UserDto?> GetByEmail(string email, CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is not null)
            return User.ToDto(user);
        return null;
    }

    public async Task<string?> GetPasswordByUsername(string username, CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
        if (user is not null)
            return user.PasswordHash;
        return null;
    }

    public async Task<UserDto?> GetByUsername(string username, CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
        if (user is not null)
            return User.ToDto(user);
        return null;
    }

    public async Task<UserDto?> GetById(int id, CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is not null)
            return User.ToDto(user);
        return null;
    }

    public async Task<UserDto?> GetByOAuthIdAsync(string oauthProvider, string oauthId, CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.OauthProvider == oauthProvider && u.OauthId == oauthId, ct);
        if (user is not null)
            return User.ToDto(user);
        return null;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var rows = await db.Users.Where(u => u.Id == id).ExecuteDeleteAsync(ct);
        return rows > 0;
    }
}
