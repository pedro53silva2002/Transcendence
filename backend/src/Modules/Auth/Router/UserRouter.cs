using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Service;

namespace Trippie.Modules.Auth.Router;

[ApiController]
[Route("api/users")]
public sealed class UserRouter(UserService service) : ControllerBase
{
    [HttpPost]
public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto, CancellationToken ct)
{
    var user = await service.CreateAsync(dto, ct);
    return CreatedAtAction(nameof(Create), user);
}

[HttpPost("search")]
public async Task<ActionResult<CursorPage<UserDto>>> Search([FromBody] SearchPayload payload, CancellationToken ct)
{
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
