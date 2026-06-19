using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Service;

namespace Trippie.Modules.Auth.Router;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UserRouter(UserService service, IUserContext userContext) : ControllerBase
{
    private static readonly JsonSerializerOptions SearchJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto, CancellationToken ct)
    {
        var user = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(Create), user);
    }

    [HttpGet("search")]
    public async Task<ActionResult<CursorPage<UserDto>>> Search([FromQuery] string q, CancellationToken ct)
    {
        try
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(q));
            var payload = JsonSerializer.Deserialize<SearchPayload>(json, SearchJsonOptions) ?? new SearchPayload();

            if (payload is null)
                return BadRequest("Invalid pauload");

            var page = await service.SearchAsync(payload, ct);
            return Ok(page);
        }
        catch (FormatException)
        {
            return BadRequest("Invalid Base64 query");
        }
        catch (JsonException)
        {
            return BadRequest("Invalid JSON payload");
        }
    }

    [HttpPut]
    public async Task<ActionResult<UserDto>> Update([FromBody] UpdateUserDto dto, CancellationToken ct)
    {
        var user = await service.UpdateAsync(userContext.UserId ?? throw new UnauthorizedException("User not authenticated."), dto, ct);
        return Ok(user);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(CancellationToken ct)
    {
		var userId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");
        await service.DeleteAsync(userId, ct);
        return NoContent();
    }
}
