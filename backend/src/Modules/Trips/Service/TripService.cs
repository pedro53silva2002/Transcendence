using System.Drawing;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Trips.Dtos;
using Trippie.Modules.Trips.Model;


namespace Trippie.Modules.Trips.Service;

public sealed class TripService(AppDbContext db, TripModel tripModel)
{
	public async Task<TripDto> CreateAsync(CreatedTripDto dto, CancellationToken ct = default)
	{
		if (string.IsNullOrWhiteSpace(dto.TripName)) throw new ValidationException("tripname", "Trip name is required");
		if (dto.StartDate == default(DateTime)) throw new ValidationException("startdate", "Start date is required");
		if (dto.EndDate == default(DateTime)) throw new ValidationException("enddate", "End date is required");

		var trip = await tripModel.CreateAsync(dto, ct);
		return trip;
	}

	public async Task<CursorPage<TripDto>> Search(SearchPayload payload, CancellationToken ct = default)
	=> await tripModel.SearchAsync(payload, ct);

	public async Task<TripDto> UpdateAsync(int id, UpdateTripDto dto, CancellationToken ct = default)
	{
		if (string.IsNullOrWhiteSpace(dto.TripName)) throw new ValidationException("tripname", "Trip name is required");
		if (dto.StartDate == default(DateTime)) throw new ValidationException("startdate", "Start date is required");
		if (dto.EndDate == default(DateTime)) throw new ValidationException("enddate", "End date is required");

		if (dto.TripName is not null || dto.StartDate != default(DateTime) || dto.EndDate != default(DateTime))
		{
			var clash = await db.Trips
				.AsNoTracking()
		}
	}
}