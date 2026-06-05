using System.Text;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Modules.Travel.Service;

namespace Trippie.Modules.Travel.Router;

[ApiController]
[Route("api/trips/{tripId}/members")]
public sealed class TripMembersRouter(TripMembersService tripMembersService, IUserContext userContext) : ControllerBase
{
	public static readonly Json
}