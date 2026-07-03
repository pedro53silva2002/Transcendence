using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Model;
using Trippie.Common.Services.MinIO;
using Trippie.Modules.Auth.Service;
using Minio;
using System.Security.Cryptography;
using System.Text;

namespace Trippie.Modules.Auth.Service;

public sealed class UserService(AppDbContext db, UserModel userModel, IMinIOService minioClient)
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
		Console.WriteLine($"Username: {dto.Username}");
		Console.WriteLine($"ProfilePhotoUrl: {dto.ProfilePhotoUrl}");
		Console.WriteLine($"ProfilePhotoPath: {dto.ProfilePhotoPath}");
		
        if (dto.ProfilePhotoUrl != null)
		{
			const string bucketName = "profile-photos";

        // 1. Procuramos o user atual para saber o nome do ficheiro antigo que está no MinIO
        var currentUser = await userModel.GetById(id, ct);
        
        if (currentUser is not null && !string.IsNullOrWhiteSpace(currentUser.ProfilePhotoUrl))
        {
            // Extrai o nome real do ficheiro antigo (ex: nome_guid.jpg)
            var oldFileName = currentUser.ProfilePhotoUrl.Split('/').Last();

            // Verifica se o ficheiro antigo existe e apaga-o para não deixar lixo no MinIO
            var found = await minioClient.FileExistsAsync(bucketName, oldFileName);
            if (found)
            {
                await minioClient.DeleteFileAsync(bucketName, oldFileName);
            }
        }

        // 2. Faz o upload do NOVO ficheiro enviado pelo Frontend
        var newFileName = $"{dto.Username}_{Guid.NewGuid()}";
        dto.ProfilePhotoPath = await minioClient.UploadFileAsync(
            bucketName, 
            newFileName, 
            dto.ProfilePhotoUrl.OpenReadStream(),  
            dto.ProfilePhotoUrl.ContentType
        );
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
    public async Task<string> GenerateUniqueUsername(string baseUsername)
    {
        if (string.IsNullOrWhiteSpace(baseUsername))
            throw new ValidationException("baseUsername", "Base username is required.");

        var prefix = NormalizeUsername(baseUsername);
        var maxPrefixLength = UsernameMaxLength - SuffixLength - 1;

        if (prefix.Length <= UsernameMaxLength)
		{
		    var existingUser = await userModel.GetByUsername(prefix);

		    if (existingUser is null)
		        return prefix;
		}

    	prefix = prefix[..Math.Min(prefix.Length, maxPrefixLength)];

		while (true)
		{
        	var suffix = GenerateRandomSuffix(SuffixLength);
        	var username = $"{prefix}_{suffix}";

			if (await userModel.GetByUsername(username) is null)
				return username;
		}
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
    // 1. Procura o utilizador
    var user = await userModel.GetById(id, ct);
    if (user is null) throw new NotFoundException($"User {id} not found.", id);

    // 2. Se o utilizador tiver foto de perfil, trata da remoção no MinIO
    if (!string.IsNullOrWhiteSpace(user.ProfilePhotoUrl))
    {
        const string bucketName = "profile-photos";
        
        // Extrai apenas o nome do ficheiro (ex: mjbalouta_e8ccd763-947b...)
        var fileName = user.ProfilePhotoUrl.Split('/').Last();

        // 🌟 Verifica se o ficheiro existe usando a string do nome
        var found = await minioClient.FileExistsAsync(bucketName, fileName);
        if (found)
        {
            // 🌟 Apaga o ficheiro diretamente usando o await correto
            await minioClient.DeleteFileAsync(bucketName, fileName);
        }
    }

    // 3. Apaga o utilizador da Base de Dados
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

	public async Task<TripStatCardsDto> GetTripStatus(int userId, CancellationToken ct = default)
	{
		var tripStatus = await userModel.GetTripStatus(userId, ct);
		if (tripStatus is null)
			return null;
		return tripStatus;
	}
}
