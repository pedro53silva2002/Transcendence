
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Service;

namespace Trippie.Modules.Auth.Router;

[ApiController]
[Authorize]
[Route("api/visited-countries")]
public sealed class VisitedCountriesRouter(
	VisitedCountriesService visitedCountriesService,
	IUserContext userContext) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<List<VisitedCountryDto>>> GetUniqueVisitedCountriesAsync(
		CancellationToken ct = default)
	{
		var userId = RequireUserId();

		var visitedCountries = await visitedCountriesService.GetUniqueVisitedCountriesAsync(userId, ct);

		var dtos = visitedCountries.Select(vc => new VisitedCountryDto
		{
			Id = vc.Id,
			Country = new CountryDto
			{
				Id = vc.Country.Id,
				Name = vc.Country.Name,
				Code = vc.Country.Code
			},
			AddedAt = vc.AddedAt,
			SourceTripId = vc.SourceTripId
		}).ToList();

		return Ok(dtos);
	}

	[HttpGet("count")]
	public async Task<ActionResult<int>> GetNumberOfVisitedCountriesAsync(CancellationToken ct = default)
	{
		var userId = RequireUserId();

		var count = await visitedCountriesService.GetNumberOfVisitedCountriesAsync(userId, ct);

		return Ok(count);
	}

	[HttpPost("sync")]
	public async Task<ActionResult<SyncVisitedCountriesResultDto>> SyncAsync(
		CancellationToken ct = default)
	{
		var userId = RequireUserId();

		var result = await visitedCountriesService.SyncFromCompletedTripsAsync(userId, ct);

		return Ok(result);
	}

	[HttpDelete("{countryId}")]
	public async Task<IActionResult> RemoveVisitedCountryAsync(int countryId, CancellationToken ct = default)
	{
		var userId = RequireUserId();

		await visitedCountriesService.RemoveVisitedCountryAsync(userId, countryId, ct);

		return NoContent();
	}

	private int RequireUserId() =>
        userContext.UserId ?? throw new UnauthorizedAccessException("User not authenticated.");
}
