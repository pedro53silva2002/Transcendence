using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Social.Dtos;

public sealed class FriendshipDto
{
	public required int Id { get; set; }
	public required int UserId1 { get; set; }
	public User User1 { get; set; } = null!;
	public required int UserId2 { get; set; }
	public User User2 { get; set; } = null!;
	public required DateTime CreatedAt { get; set; }
}

public sealed class CreateFriendshipDto
{
	public required int UserId1 { get; set; }
	public required int UserId2 { get; set; }
}

public sealed class FriendDto
{
	public required int FriendId { get; set; }
	public required string Username { get; set; }
    public string? ProfilePhotoUrl { get; set; }
}
