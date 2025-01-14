namespace PicPay.Api.Features.Adm.GetWallet;

[ApiController, AuthCustomer]
[Consumes("application/json"), Produces("application/json")]
public class GetWalletController(GetWalletService service) : ControllerBase
{
    /// <summary>
    /// Carteira
    /// </summary>
    /// <remarks>
    /// Retorna a Carteira do Cliente.
    /// </remarks>
    [HttpGet("customer/wallet")]
    public async Task<IActionResult> GetWallet()
    {
        var result = await service.GetWallet(User.Id());

        return Ok(result);
    }
}
