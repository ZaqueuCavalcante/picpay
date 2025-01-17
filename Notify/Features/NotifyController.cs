using Microsoft.AspNetCore.Mvc;

namespace PicPay.Notify.Features;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class NotifyController : ControllerBase
{
    [HttpPost("api/v1/notify")]
    public IActionResult Notify([FromQuery] long? amount)
    {
        bool notify;
        if (amount != null)
        {
            long[] fails = [58_90];
            notify = !fails.Contains(amount.Value);
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
