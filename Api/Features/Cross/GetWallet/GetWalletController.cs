using Microsoft.AspNetCore.Authorization;

namespace PicPay.Api.Features.Cross.GetWallet;

[ApiController, Authorize]
[EnableRateLimiting(nameof(RateLimiterSettings.Medium))]
[Consumes("application/json"), Produces("application/json")]
public class GetWalletController(GetWalletService service) : ControllerBase
{
    /// <summary>
    /// 💰 Carteira
    /// </summary>
    /// <remarks>
    /// Retorna a Carteira do usuário.
    /// </remarks>
    [HttpGet("wallet")]
    [ProducesResponseType(typeof(GetWalletOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> GetWallet()
    {
        var result = await service.GetWallet(User.Id());

        return Ok(result);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<GetWalletOut>
{
    public IEnumerable<SwaggerExample<GetWalletOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"GetWalletOut",
			new GetWalletOut
			{
				Id = Guid.NewGuid(),
                Balance = 12_34
			}
		);
    }
}
