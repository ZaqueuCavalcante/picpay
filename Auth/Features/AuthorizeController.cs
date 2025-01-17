using Microsoft.AspNetCore.Mvc;

namespace PicPay.Auth.Features;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class AuthorizeController : ControllerBase
{
    [HttpGet("api/v2/authorize")]
    public IActionResult Authorize([FromQuery] long? amount)
    {
        if (amount != null && amount.Value == 374_58) return new StatusCodeResult(504);

        bool authorize;
        if (amount != null)
        {
            long[] fails = [159_75];
            authorize = !fails.Contains(amount.Value);
        }
        else
        {
            authorize = new Random().NextDouble() > 0.5;
        }

        return Ok(new AuthorizeOut(authorize));
    }
}
