using System.Globalization;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.Authentication.Security;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;

namespace Trippie.Modules.Travel.Model;

public sealed class TripMembers
{
	public required int Id { get; set; }
	public required int TripId { get; set; }
	public required int UserId { get; set; }
	public required TripMemberRole Role { get; set; }
	public DateTime JoinedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public static TripMembersDto ToDto(TripMembers tm) => new()
	{
		Id = tm.Id,
		TripId = tm.TripId,
		UserId = tm.UserId,
		Role = tm.Role,
		JoinedAt = tm.JoinedAt,
		UpdatedAt = tm.UpdatedAt,
	};
}

public sealed class TripMembersModel(AppDbContext db)
{
	public async Task<IReadOnlyList<TripMembersDto>> CreateAsync(int callerId, CreateTripMembersDto dto, CancellationToken ct = default)
	{
		var userIds = dto.UserIds.Distinct().ToArray();
	    
		if (userIds.Distinct().Count() != userIds.Length)
	        throw new SearchValidationException("Duplicate user IDs were provided in the request.");

		if (userIds.Length == 0)
	        throw new SearchValidationException("At least one user ID must be provided.");

		if (userIds.Count(id => id == callerId) != 1)
			throw new SearchValidationException("The trip creator must appear exactly once in the member list.");

		var existingUserIds = await db.TripMembers
        .Where(tm => tm.TripId == dto.TripId && userIds.Contains(tm.UserId))
        .Select(tm => tm.UserId)
        .ToListAsync(ct);

		if (existingUserIds.Count != 0)
        	throw new SearchValidationException(
				$"These users are already members of the trip: {string.Join(", ", existingUserIds)}.");

		var members = userIds.Select(userId => new TripMembers
    	{
	    	Id = 0,
    	    TripId = dto.TripId,
    	    UserId = userId,
    	    Role = userId == callerId ? TripMemberRole.Admin : TripMemberRole.Member,
    	    JoinedAt = DateTime.UtcNow
    	}).ToList();

		await db.BulkInsertAsync(members, cancellationToken: ct);
		return members.Select(TripMembers.ToDto).ToList();
	}

	public async Task 
}

// validate if more than 1 admin exists in the trip before changing role permission
		// var isAdmin = await db.TripMembers.AnyAsync(
		//     tm => tm.TripId == dto.TripId &&
		//         tm.UserId == callerId &&
		//         tm.Role == TripMemberRole.Admin, ct);
		// if (!isAdmin)
		//     throw new SearchValidationException("Only admins can add members to the trip.");