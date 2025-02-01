using Microsoft.AspNetCore.Authorization;

namespace PicPay.Api.Features.Cross.GetExtract;

[ApiController, Authorize]
[Consumes("application/json"), Produces("application/json")]
public class GetExtractController(GetExtractService service) : ControllerBase
{
    /// <summary>
    /// Extrato
    /// </summary>
    /// <remarks>
    /// Retorna o extrato de transações do usuário.
    /// </remarks>
    [HttpGet("extract")]
    public async Task<IActionResult> Get()
    {
        var result = await service.Get(User.WalletId());

        return Ok(result);
    }
}
