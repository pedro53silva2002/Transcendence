using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Trippie.Modules.Social.Service;

[ApiController]
[Authorize]
[Route("api/friend-requests")]
public sealed class FriendRequestRouter(FriendRequestService friendRequestService, IUserContext userContext) : ControllerBase
{
}