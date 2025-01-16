using Microsoft.AspNetCore.Authorization;

namespace PicPay.Api.Features.Cross.GetWallet;

[ApiController, Authorize]
[Consumes("application/json"), Produces("application/json")]
public class GetWalletController(GetWalletService service) : ControllerBase
{
    /// <summary>
    /// Carteira
    /// </summary>
    /// <remarks>
    /// Retorna a Carteira do usuário.
    /// </remarks>
    [HttpGet("wallet")]
    public async Task<IActionResult> GetWallet()
    {
        var result = await service.GetWallet(User.Id());

        return Ok(result);
    }
}
