using Microsoft.AspNetCore.Authorization;

namespace PicPay.Api.Features.Cross.GetNotifications;

[ApiController, Authorize]
[Consumes("application/json"), Produces("application/json")]
public class GetNotificationsController(GetNotificationsService service) : ControllerBase
{
    /// <summary>
    /// Notificações
    /// </summary>
    /// <remarks>
    /// Retorna as Notificações do usuário.
    /// </remarks>
    [HttpGet("notifications")]
    public async Task<IActionResult> Get()
    {
        var result = await service.Get(User.Id());

        return Ok(result);
    }
}
