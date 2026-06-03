using System.Drawing;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Modules.Travel.Model;


namespace Trippie.Modules.Travel.Service;

public sealed class TripService(TripModel tripModel)
{
	public async Task<TripDto> CreateAsync(CreatedTripDto dto, CancellationToken ct = default)
	{
		ValidateTrip(dto.TripName, dto.Description, dto.budget, dto.StartDate, dto.EndDate);
		var trip = await tripModel.CreateAsync(dto, ct);
		return trip;
	}

	public async Task<CursorPage<TripDto>> SearchAsync(SearchPayload payload, CancellationToken ct = default)
	=> await tripModel.SearchAsync(payload, ct);

	public async Task<TripDto> UpdateAsync(int id, UpdateTripDto dto, CancellationToken ct = default)
	{
		ValidateTrip(dto.TripName, dto.Description, dto.budget, dto.StartDate, dto.EndDate);
		var trip = await tripModel.UpdateAsync(id, dto, ct) ?? throw new NotFoundException($"Trip {id} not found.", id);

		return Trip.ToDto(trip);
	}

	public async Task<TripDto?> GetById(int id, CancellationToken ct = default)
	{
		var res = await tripModel.GetById(id, ct);
		if (res is null)
			return null;
		return res;
	}

	public async Task DeleteAsync(int id, CancellationToken ct = default)
	{
		var delete = await tripModel.DeleteAsync(id, ct);
		if (!delete) throw new NotFoundException($"Trip {id} not found.", id);
	}

	private void ValidateTrip(string tripname, string description, int budget, DateTime startDate, DateTime endDate)
	{
		if (string.IsNullOrWhiteSpace(tripName)) throw new ValidationException("tripname", "Trip name is required");
		if (startDate == default(DateTime)) throw new ValidationException("startdate", "Start date is required");
		if (startDate == default(DateTime)) throw new ValidationException("startdate", "Start date is required");
		if (endDate == default(DateTime)) throw new ValidationException("enddate", "End date is required");
		if (budget < 0) throw new ValidationException("budget", "Budget value invalid.");
		if (endDate < startDate) throw new ValidationException("endDate, startDate", "End date cannot be before start date.");
	}
}
