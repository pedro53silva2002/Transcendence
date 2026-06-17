namespace Trippie.Modules.Social.Dtos;

public sealed class FriendshipDto
{
	public required int Id { get; set; }
	public required int UserId1 { get; set; }
	public required int UserId2 { get; set; }
	public required DateTime CreatedAt { get; set; }
}
