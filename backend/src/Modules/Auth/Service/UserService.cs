using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Model;
using Trippie.Common.Services.MinIO;
using Trippie.Modules.Auth.Service;
using Minio;

namespace Trippie.Modules.Auth.Service;

public sealed class UserService(AppDbContext db, UserModel userModel, IMinIOService minioClient)
{
    public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Email)) throw new ValidationException("email", "Email is required.");
        if (string.IsNullOrWhiteSpace(dto.Username)) throw new ValidationException("username", "Username is required.");

        var existsUsername = await userModel.GetByUsername(dto.Username, ct);
        var existsEmails = await userModel.GetByEmail(dto.Email, ct);

        if (existsEmails is not null || existsUsername is not null)
        {
            var field = existsEmails is not null ? "email" : "username";
            throw new ConflictException($"User with this {field} already exists.");
        }
		if (dto.OAuthProvider == "none")
			dto.ProfilePhotoUrl = await minioClient.UploadFileAsync("profile-photos", $"{dto.Username}_{Guid.NewGuid()}", new MemoryStream(), "application/octet-stream");
        var user = await userModel.CreateAsync(dto, ct);
        return user;
    }

    public async Task<CursorPage<UserDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
	{
		var res = await userModel.SearchAsync(payload, ct);
		/*Console.WriteLine($"ProfilePhotoUrl: {res.Content[0].ProfilePhotoUrl}");
		Console.WriteLine($"Path: {res.Content[0].ProfilePhotoUrl.Split('/')[0]}");
		Console.WriteLine($"Path: {res.Content[0].ProfilePhotoUrl.Split('/')[1]}");*/
		res.Content.Select(u => 
		{
			if (u.ProfilePhotoUrl is not null && u.ProfilePhotoUrl[0] == '/')
				u.ProfilePhotoUrl = minioClient.GetObjectUrl(
					u.ProfilePhotoUrl.Split('/')[1], 
					u.ProfilePhotoUrl.Split('/')[2])
					.GetAwaiter().GetResult();
			return u;
		}).ToList();
		return res;
	}

    public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default)
    {
        if (dto.Email is not null && string.IsNullOrWhiteSpace(dto.Email))
            throw new ValidationException("email", "Email can not be empty.");
        if (dto.Username is not null && string.IsNullOrWhiteSpace(dto.Username))
            throw new ValidationException("username", "Username can not be blank.");
		if (dto.Password is not null)
			AuthService.PasswordVerification(dto.Password);

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
		if (dto.ProfilePhotoUrl != null)
		{
			var found = await minioClient.FileExistsAsync("profile-photos", dto.ProfilePhotoUrl);
			if (found)
				await minioClient.DeleteFileAsync("profile-photos", dto.ProfilePhotoUrl);
			dto.ProfilePhotoUrl = await minioClient.UploadFileAsync("profile-photos", $"{dto.Username}_{Guid.NewGuid()}", new MemoryStream(), "application/octet-stream");
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
		if (res is null)
			return null;
		if (res.ProfilePhotoUrl is not null && res.ProfilePhotoUrl[0] == '/')
		{
			var path_url = res.ProfilePhotoUrl.Split('/');
			res.ProfilePhotoUrl = await minioClient.GetObjectUrl(path_url[1], path_url[2]);
		}
        return res;
    }

	 public async Task<UserDto?> GetByUsername(string username, CancellationToken ct = default)
    {
        if (username is null) throw new ValidationException("username", $"Username cannot be empty.");

        var res = await userModel.GetByUsername(username, ct);
		if (res is null)
			return null;
		if (res.ProfilePhotoUrl is not null && res.ProfilePhotoUrl[0] == '/')
		{
			var path_url = res.ProfilePhotoUrl.Split('/');
			res.ProfilePhotoUrl = await minioClient.GetObjectUrl(path_url[1], path_url[2]);
		}
        return res;
    }

    public async Task<UserDto?> GetById(int id, CancellationToken ct = default)
    {
        var res = await userModel.GetById(id, ct);
        if (res is null)
            return null;
		if (res.ProfilePhotoUrl is not null && res.ProfilePhotoUrl[0] == '/')
		{
			var path_url = res.ProfilePhotoUrl.Split('/');
			res.ProfilePhotoUrl = await minioClient.GetObjectUrl(path_url[1], path_url[2]);
		}

        return res;
    }
    public async Task<string> GenerateUniqueUsername(string baseUsername, CancellationToken ct = default)
    {
        var searchResult = await userModel.SearchAsync(new SearchPayload
        {
            Filters = [new FilterCriterion("username", FilterOperator.StartsWith, baseUsername)],
            Sort = [new SortCriterion("username", SortDirection.Asc)],
            Page = new CursorPageRequest
            {
                PageSize = 100
            }
        }, ct);
        var existingUsers = searchResult?.Content;

        if (existingUsers is null || !existingUsers.Any())
            return baseUsername;

        int number = 1;
        while (existingUsers.Any(u => u.Username.Equals($"{baseUsername}{number}", StringComparison.OrdinalIgnoreCase)))
            number++;
        var candidateUsername = $"{baseUsername}{number}";
        return candidateUsername;
    }

    public async Task<UserDto?> GetByOAuthIdAsync(string oauthProvider, string oauthId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(oauthId))
            throw new ValidationException("oauthId", "OAuth ID is required.");

		var res = await userModel.GetByOAuthIdAsync(oauthProvider, oauthId, ct);
		if (res is null)
			return null;
		if (res.ProfilePhotoUrl is not null && res.ProfilePhotoUrl[0] == '/')
		{
			var path_url = res.ProfilePhotoUrl.Split('/');
			res.ProfilePhotoUrl = await minioClient.GetObjectUrl(path_url[1], path_url[2]);
		}
        return res;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
		var user = await userModel.GetById(id, ct);
		if (user is null) throw new NotFoundException($"User {id} not found.", id);
		if (user.ProfilePhotoUrl is not null)
		{
			var found = await minioClient.FileExistsAsync("profile-photos", user.ProfilePhotoUrl);
			if (found)
			{
				if (user.ProfilePhotoUrl is not null)
				{
					var path_url = user.ProfilePhotoUrl.Split('/');
					await minioClient.DeleteFileAsync(path_url[1], path_url[2]);
				}
			}
		}
        var deleted = await userModel.DeleteAsync(id, ct);
        if (!deleted) throw new NotFoundException($"User {id} not found for deleting.", id);
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
