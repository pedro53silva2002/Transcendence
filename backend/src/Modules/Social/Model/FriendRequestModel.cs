using Microsoft.EntityFrameworkCore;
using Trippie.Modules.Social.Config;
using Trippie.Modules.Social.Dtos;

namespace Trippie.Modules.Social.Model;

public sealed class FriendRequest
{
	public required int Id { get; set; }
	public required int SenderId { get; set; }
	public required int ReceiverId { get; set; }
	public required FriendRequestStatus Status { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }

	public static FriendRequestDto ToDto(FriendRequest fr) => new()
	{
		Id = fr.Id,
		SenderId = fr.SenderId,
		ReceiverId = fr.ReceiverId,
		Status = fr.Status,
		CreatedAt = fr.CreatedAt,
		UpdatedAt = fr.UpdatedAt,
	};
}