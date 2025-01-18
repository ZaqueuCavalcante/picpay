using Microsoft.AspNetCore.Mvc;

namespace PicPay.Auth.Features;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class AuthorizeController : ControllerBase
{
    [HttpGet("api/v2/authorize")]
    public IActionResult Authorize([FromQuery] long? amount)
    {
        if (amount != null && amount.Value == 100_00) return new StatusCodeResult(504);

        bool authorize;
        if (amount != null)
        {
            long[] fails = [200_00];
            authorize = !fails.Contains(amount.Value);
        }
        else
        {
            authorize = new Random().NextDouble() > 0.5;
        }

        return Ok(new AuthorizeOut(authorize));
    }
}
