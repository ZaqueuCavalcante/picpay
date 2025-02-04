using Microsoft.AspNetCore.Authorization;

namespace PicPay.Api.Features.Cross.GetNotifications;

[ApiController, Authorize]
[EnableRateLimiting(nameof(RateLimiterSettings.Medium))]
[Consumes("application/json"), Produces("application/json")]
public class GetNotificationsController(GetNotificationsService service) : ControllerBase
{
    /// <summary>
    /// 🔔 Notificações
    /// </summary>
    /// <remarks>
    /// Retorna as Notificações do usuário.
    /// </remarks>
    [HttpGet("notifications")]
    [ProducesResponseType(typeof(List<GetNotificationOut>), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var result = await service.Get(User.Id());

        return Ok(result);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<List<GetNotificationOut>>
{
    public IEnumerable<SwaggerExample<List<GetNotificationOut>>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Cliente",
			new List<GetNotificationOut>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Status = NotificationStatus.Success,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Message = "Você recebeu uma transferência de R$ 6,80 de Marisvaldison Gomes",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Status = NotificationStatus.Success,
                    CreatedAt = DateTime.Now.AddDays(-3),
                    Message = "Bônus de Boas-Vindas no valor de R$ 10,00",
                }
            }
		);

        yield return SwaggerExample.Create(
			"Lojista",
			new List<GetNotificationOut>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Status = NotificationStatus.Success,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Message = "Você recebeu uma transferência de R$ 1,23 de Valdineida Silva",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Status = NotificationStatus.Success,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    Message = "Você recebeu uma transferência de R$ 91,54 de Gilsinho Trembolono",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Status = NotificationStatus.Success,
                    CreatedAt = DateTime.Now.AddDays(-3),
                    Message = "Você recebeu uma transferência de R$ 58,00 de Marisvaldison Gomes",
                },
            }
		);
    }
}
