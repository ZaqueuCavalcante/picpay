namespace PicPay.Api.Features.Adm.Transfer.GetSignUpBonus;

[ApiController, AuthCustomer]
[Consumes("application/json"), Produces("application/json")]
public class GetSignUpBonusController(GetSignUpBonusService service) : ControllerBase
{
    /// <summary>
    /// Bônus de cadastro
    /// </summary>
    /// <remarks>
    /// Dá ao Cliente um Bônus em dinheiro por ter se cadastrado no sistema. <br/>
    /// Esse endpoint é idempotente.
    /// </remarks>
    [HttpPost("customer/sign-up-bonus")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetSignUpBonus()
    {
        var result = await service.GetSignUpBonus(User.Id());

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
