using System.Drawing;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Modules.Travel.Model;


namespace Trippie.Modules.Travel.Service;

public sealed class TripService(TripModel tripModel, TripMembersModel tripMembersModel)
{
	public async Task<TripDto> CreateAsync(CreateTripDto dto, int userId, CancellationToken ct = default)
	{
		ValidateTrip(dto.TripName, dto.Description, dto.Budget, dto.StartDate, dto.EndDate);
		var trip = await tripModel.CreateAsync(dto, userId, ct);
		dto.Members.TripId = trip.Id;
		var members = await new TripMembersService(tripMembersModel).CreateAsync(userId, dto.Members, ct);
		trip.Members = [.. members];
		return trip;
	}

	public async Task<CursorPage<TripDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
	=> await tripModel.SearchAsync(payload, ct);

	public async Task<TripDto> UpdateAsync(int userId, int id, UpdateTripDto dto, CancellationToken ct = default)
	{
		ValidateTrip(dto.TripName, dto.Description, dto.Budget, dto.StartDate, dto.EndDate);
		var trip = await tripModel.UpdateAsync(userId, id, dto, ct) ?? throw new NotFoundException($"Trip {id} not found.", id);

		return trip;
	}

	public async Task<TripDto?> GetById(int id, CancellationToken ct = default)
	{
		var res = await tripModel.GetById(id, ct);
		if (res is null)
			return null;
		return res;
	}

	public async Task DeleteAsync(int userId, int id, CancellationToken ct = default)
	{
		var delete = await tripModel.DeleteAsync(userId, id, ct);
		if (!delete) throw new NotFoundException($"Trip {id} not found.", id);
	}

	private void ValidateTrip(string tripName, string? description, int budget, DateTime startDate, DateTime endDate)
	{
		if (string.IsNullOrWhiteSpace(tripName)) throw new ValidationException("tripname", "Trip name is required");
		if (tripName.Length > 25 || tripName.Length < 3) throw new ValidationException("tripname", "Trip name must be between 3 and 25 characters.");
		if (startDate == default(DateTime)) throw new ValidationException("startdate", "Start date is required");
		if (endDate == default(DateTime)) throw new ValidationException("enddate", "End date is required");
		if (endDate <= startDate) throw new ValidationException("endDate, startDate", "End date cannot be before or equal to start date.");
		if (budget < 0) throw new ValidationException("budget", "Budget value invalid.");
		if (description != null && description.Length > 250) throw new ValidationException("description", "Description cannot be longer than 250 characters.");
	}
}
