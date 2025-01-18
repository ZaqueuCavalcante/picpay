using Microsoft.AspNetCore.Mvc;
using PicPay.Notify.Extensions;

namespace PicPay.Notify.Features;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class NotifyController : ControllerBase
{
    [HttpPost("api/v1/notify")]
    public IActionResult Notify([FromBody] NotifyIn data)
    {
        bool notify;
        if (data != null)
        {
            long[] fails = [58_90];
            var amount = long.Parse(data.Message.OnlyNumbers());
            notify = !fails.Contains(amount);
        }
        else
        {
            notify = new Random().NextDouble() > 0.5;
        }

        if (notify) return NoContent();

        return GatewayTimeout();
    }

    private JsonResult GatewayTimeout()
    {
        Response.StatusCode = StatusCodes.Status504GatewayTimeout;
        return new JsonResult(NotifyOut.NewError());
    }
}
