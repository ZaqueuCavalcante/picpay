using Microsoft.AspNetCore.Mvc;
using PicPay.Vendors.Extensions;

namespace PicPay.Vendors.Notify;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class NotifyController : ControllerBase
{
    [HttpPost("api/v1/notify")]
    public IActionResult Notify([FromBody] NotifyIn? data)
    {
        bool notify;
        if (data != null)
        {
            long[] fails = [50_00];
            var value = data.Message.OnlyNumbers();
            var amount = long.Parse(value.Length > 0 ? value : "0");
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
