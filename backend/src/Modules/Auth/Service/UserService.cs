using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Service;

public sealed class UserService(AppDbContext db, UserModel userModel)
{
    public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Email)) throw new ValidationException("email", "Email is required.");
        if (string.IsNullOrWhiteSpace(dto.Username)) throw new ValidationException("username", "Username is required.");
        if (string.IsNullOrWhiteSpace(dto.Password)) throw new ValidationException("password", "Password is required.");

        var clash = await db.Users
            .AsNoTracking()
            .Where(u => u.Email == dto.Email || u.Username == dto.Username)
            .Select(u => new { u.Email, u.Username })
            .FirstOrDefaultAsync(ct);

        if (clash is not null)
        {
            var field = string.Equals(clash.Email, dto.Email, StringComparison.OrdinalIgnoreCase) ? "email" : "username";
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

        return User.ToDto(user);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var deleted = await userModel.DeleteAsync(id, ct);
        if (!deleted) throw new NotFoundException($"User {id} not found.", id);
    }
}
