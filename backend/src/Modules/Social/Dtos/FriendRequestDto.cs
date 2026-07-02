using Trippie.Modules.Social.Config;

namespace Trippie.Modules.Social.Dtos;

public sealed class CreateFriendRequestDto
{
	public required int ReceiverId { get; set; }
}

public sealed class FriendRequestDto
{
	public required int Id { get; set; }
	public required int SenderId { get; set; }
	public required string SenderUsername { get; set; }
	public string? SenderProfilePhotoUrl { get; set; }
	public required int ReceiverId { get; set; }
	public required FriendRequestStatus Status { get; set; }
	public required DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}

public sealed class CreateFriendRequestResult
{
	public FriendRequestDto? FriendRequest { get; set; }
	public FriendshipDto? Friendship { get; set; }
	public bool WasAutoAccepted => Friendship is not null;
}