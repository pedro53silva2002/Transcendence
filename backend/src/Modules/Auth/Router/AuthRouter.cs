using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Service;

namespace Trippie.Modules.Auth.Router;

[ApiController]
[Route("api/auth")]
public sealed class AuthRouter(AuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto, CancellationToken ct)
        => await service.Register(dto, ct);

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<MeDto>> Me(CancellationToken ct)
        => await service.GetMe(User, ct);
}
