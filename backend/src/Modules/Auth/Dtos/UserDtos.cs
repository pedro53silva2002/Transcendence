using Trippie.Common.Services.Search.Model;

namespace Trippie.Modules.Auth.Dtos;

public sealed class CreateUserDto
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public sealed class UserDto
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public string? OAuthProvider { get; set; }
    public string? OAuthId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public sealed class UpdateUserDto
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePhotoUrl { get; set; }
}

public sealed class UserSearchDto
{
    public IReadOnlyList<FilterCriterion>? Filters { get; set; }
    public IReadOnlyList<SortCriterion>? Sort { get; set; }
    public CursorPageRequest? Page { get; set; }
}
