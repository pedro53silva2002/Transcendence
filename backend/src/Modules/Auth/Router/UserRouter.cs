using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Service;

namespace Trippie.Modules.Auth.Router;

[ApiController]
[Route("api/users")]
public sealed class UserRouter(UserService service) : ControllerBase
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
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(q));
        var payload = JsonSerializer.Deserialize<SearchPayload>(json, SearchJsonOptions) ?? new SearchPayload();
        var page = await service.SearchAsync(payload, ct);
        return Ok(page);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto, CancellationToken ct)
    {
        var user = await service.UpdateAsync(id, dto, ct);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
