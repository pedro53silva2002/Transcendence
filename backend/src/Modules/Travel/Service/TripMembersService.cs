using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Modules.Travel.Model;

namespace Trippie.Modules.Travel.Service;

public sealed class TripMembersService(TripMembersModel tripMembersModel)
{
	public async Task<IReadOnlyList<TripMembersDto>> CreateAsync(int callerId, CreateTripMembersDto dto, CancellationToken ct = default)
	{
		var createdMembers = await tripMembersModel.CreateAsync(callerId, dto, ct);
		return createdMembers;
	}

	public async Task<CursorPage<TripMembersDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
	=> await tripMembersModel.SearchAsync(payload, ct);
	
	public async Task<TripMembersDto> UpdateAsync(int userId, int id, UpdateTripMembersDto dto, CancellationToken ct = default)
	{
		var tripmember = await tripMembersModel.UpdateAsync(userId, id, dto, ct) ?? throw new NotFoundException($"Trip {id} not found.", id);

		return tripmember;
	}

	public async Task<TripMembersDto?> GetById(int id, CancellationToken ct = default)
	{
		var res = await tripMembersModel.GetById(id, ct);
		if (res is null)
			return null;
		return res;
	}

	public async Task DeleteAsync(int MyId, int DeletingId, CancellationToken ct = default)
	{
		var delete = await tripMembersModel.DeleteAsync(MyId, DeletingId, ct);
		if (!delete)
			throw new NotFoundException($"Trip member {DeletingId} not found.", DeletingId);
	}
}
