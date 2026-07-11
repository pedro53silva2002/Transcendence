using System.Data;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Model;
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
	public User? User { get; set; }

	public static TripMembersDto ToDto(TripMembers tm, User user) => new()
	{
		Id = tm.Id,
		TripId = tm.TripId,
		UserId = tm.UserId,
		DisplayName = user.DisplayName,
		Username = user.Username,
		ProfilePicture = user.ProfilePhotoUrl,
		Role = tm.Role,
		JoinedAt = tm.JoinedAt,
		UpdatedAt = tm.UpdatedAt,
	};
}

public sealed class TripMembersModel(AppDbContext db)
{
	private static void ValidateCreateRequest(int callerId, CreateTripMembersDto dto, IReadOnlyList<int> userIds)
	{
		if (dto.TripId <= 0)
			throw new SearchValidationException("Trip ID must be greater than zero.");

		if (userIds.Count == 0)
			throw new SearchValidationException("At least one user ID must be provided.");

		if (userIds.Distinct().Count() != userIds.Count)
			throw new SearchValidationException("Duplicate user IDs were provided in the request.");

		if (userIds.Count(id => id == callerId) != 1)
		 	throw new SearchValidationException("The trip creator must appear exactly once in the member list.");
	}

	private async Task<int> CountAdminsAsync(int tripId, int excludedMemberId, CancellationToken ct)
	{
		return await db.TripMembers.CountAsync(
			tm => tm.TripId == tripId && tm.Role == TripMemberRole.Admin && tm.Id != excludedMemberId,
			ct);
	}

	private async Task EnsureCallerIsTripAdminAsync(int tripId, int callerId, CancellationToken ct)
	{
		var isAdmin = await db.TripMembers.AnyAsync(
			tm => tm.TripId == tripId && tm.UserId == callerId && tm.Role == TripMemberRole.Admin,
			ct);

		if (!isAdmin)
			throw new SearchValidationException("Only admins can update members of the trip.");
	}

	private async Task EnsureOperationKeepsAtLeastOneAdminAsync(TripMembers member, CancellationToken ct)
	{
		if (member.Role != TripMemberRole.Admin)
			return;

		var remainingAdmins = await CountAdminsAsync(member.TripId, member.Id, ct);
		if (remainingAdmins == 0)
			throw new ValidationException("oneMember", "The trip must always have at least one admin.");
	}

	private async Task<T> ExecuteInSerializableTransactionAsync<T>(Func<Task<T>> action, CancellationToken ct)
	{
		await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);

		try
		{
			var result = await action();
			await transaction.CommitAsync(ct);
			return result;
		}
		catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.SerializationFailure)
		{
			await transaction.RollbackAsync(ct);
			throw new ConflictException("The trip was modified concurrently. Please retry the operation.");
		}
		catch
		{
			await transaction.RollbackAsync(ct);
			throw;
		}
	}

	public async Task<IReadOnlyList<TripMembersDto>> CreateAsync(int callerId, CreateTripMembersDto dto, CancellationToken ct = default)
	{
		ArgumentNullException.ThrowIfNull(dto.UserIds);

		var userIds = dto.UserIds.ToArray();

		var hasExistingMembers = await db.TripMembers.AnyAsync(tm => tm.TripId == dto.TripId, ct);
		if (!hasExistingMembers)
			ValidateCreateRequest(callerId, dto, userIds);
		else
		{
			if (dto.TripId <= 0)
				throw new SearchValidationException("Trip ID must be greater than zero.");
			if (userIds.Length == 0)
				throw new SearchValidationException("At least one user ID must be provided.");
			if (userIds.Distinct().Count() != userIds.Length)
				throw new SearchValidationException("Duplicate user IDs were provided in the request.");
		}

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

		await db.TripMembers.AddRangeAsync(members, ct);
		await db.SaveChangesAsync(ct);

		var usersById = await db.Users
			.Where(u => userIds.Contains(u.Id))
			.ToDictionaryAsync(u => u.Id, ct);

		return [.. members.Select(member => TripMembers.ToDto(member, usersById[member.UserId]))];
	}

	public async Task<CursorPage<TripMembersDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
	{
		var res = await new SearchQueryBuilder<TripMembers>(db.TripMembers)
			.WithKey("id", tm => tm.Id)
			.AddFilters(payload.Filters, field => field.ToLowerInvariant() switch
			{
				"id" => tm => tm.Id,
				"tripid" => tm => tm.TripId,
				"userid" => tm => tm.UserId,
				"role" => tm => tm.Role,
				"joinedat" => tm => tm.JoinedAt,
				_ => throw new SearchValidationException($"Invalid filter field: {field}")
			})
			.SetOrderBy(payload.Sort, field => field.ToLowerInvariant() switch
			{
				"id" => tm => tm.Id,
				"tripid" => tm => tm.TripId,
				"userid" => tm => tm.UserId,
				"role" => tm => tm.Role,
				"joinedat" => tm => tm.JoinedAt,
				_ => throw new SearchValidationException($"Invalid sort field: {field}")
			})
			.SetCursorPagination(payload.Page)
			.RunAsync(tm => new TripMembersDto
			{
				Id = tm.Id,
				TripId = tm.TripId,
				UserId = tm.UserId,
				DisplayName = tm.User!.DisplayName,
				ProfilePicture = tm.User.ProfilePhotoUrl,
				Username = tm.User.Username,
				Role = tm.Role,
				JoinedAt = tm.JoinedAt,
				UpdatedAt = tm.UpdatedAt,
			}, ct);

		return res;
	}

	private async Task<TripMembersDto> ToDtoAsync(TripMembers member, CancellationToken ct)
	{
		var user = await db.Users.FirstAsync(u => u.Id == member.UserId, ct);
		return TripMembers.ToDto(member, user);
	}

	public Task<TripMembersDto?> UpdateAsync(int adminId, int id, UpdateTripMembersDto dto, CancellationToken ct = default)
	{
		return ExecuteInSerializableTransactionAsync(async () =>
		{
			var member = await db.TripMembers.FirstOrDefaultAsync(tm => tm.Id == id, ct);
			if (member == null)
				return null;

			await EnsureCallerIsTripAdminAsync(member.TripId, adminId, ct);

			var isSelfRoleChangeToMember = member.UserId == adminId && member.Role == TripMemberRole.Admin && dto.Role == TripMemberRole.Member;
			if (isSelfRoleChangeToMember)
				await EnsureOperationKeepsAtLeastOneAdminAsync(member, ct);

			member.Role = dto.Role;
			member.UpdatedAt = DateTime.UtcNow;
			await db.SaveChangesAsync(ct);

			return await ToDtoAsync(member, ct);
		}, ct);
	}

	public async Task<TripMembersDto?> GetById(int id, CancellationToken ct = default)
	{
		var member = await db.TripMembers
			.FirstOrDefaultAsync(tm => tm.Id == id, ct);
		return member == null ? null : await ToDtoAsync(member, ct);
	}

	public Task<bool> DeleteAsync(int myId, int deletingId, CancellationToken ct = default)
	{
		return ExecuteInSerializableTransactionAsync(async () =>
		{
			var member = await db.TripMembers.FirstOrDefaultAsync(tm => tm.Id == deletingId, ct);
			if (member == null)
				return false;

			await EnsureCallerIsTripAdminAsync(member.TripId, myId, ct);

			var isSelfDelete = deletingId == myId && member.Role == TripMemberRole.Admin;
			if (isSelfDelete)
				await EnsureOperationKeepsAtLeastOneAdminAsync(member, ct);

			db.TripMembers.Remove(member);
			await db.SaveChangesAsync(ct);
			return true;
		}, ct);
	}

	public async Task<bool> IsMemberAsync(int userId, int tripId, CancellationToken ct = default)
    	=> await db.Set<TripMembers>().AnyAsync(tm => tm.TripId == tripId && tm.UserId == userId, ct);
}
