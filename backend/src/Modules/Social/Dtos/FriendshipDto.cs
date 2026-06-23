namespace Trippie.Modules.Social.Dtos;

public sealed class FriendshipDto
{
	public required int Id { get; set; }
	public required int FriendId { get; set; }
	public required DateTime CreatedAt { get; set; }
}

public sealed class CreateFriendshipDto
{
	public required int FriendId { get; set; }
}

public sealed class FriendDto
{
	public required int FriendId { get; set; }
	public required string Username { get; set; }
    public string? ProfilePhotoUrl { get; set; }
}
