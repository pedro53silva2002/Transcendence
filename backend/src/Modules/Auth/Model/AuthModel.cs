using Trippie.Modules.Auth.Dtos;

namespace Trippie.Modules.Auth.Model;

public sealed class RefreshToken
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required string Token { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;
}
