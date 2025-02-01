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
    [ProducesResponseType(typeof(List<GetExtractOut>), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var result = await service.Get(User.WalletId());

        return Ok(result);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<List<GetExtractOut>>
{
    public IEnumerable<SwaggerExample<List<GetExtractOut>>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Cliente",
			new List<GetExtractOut>()
            {
                new()
                {
                    Amount = -6_80,
                    Id = Guid.NewGuid(),
                    Other = "Marisvaldison Gomes",
                    Type = TransactionType.Transfer,
                    CreatedAt = DateTime.Now.AddDays(-1),
                },
                new()
                {
                    Amount = 10_00,
                    Other = "PicPay",
                    Id = Guid.NewGuid(),
                    Type = TransactionType.WelcomeBonus,
                    CreatedAt = DateTime.Now.AddDays(-3),
                }
            }
		);

        yield return SwaggerExample.Create(
			"Lojista",
			new List<GetExtractOut>()
            {
                new()
                {
                    Amount = 1_23,
                    Id = Guid.NewGuid(),
                    Other = "Valdineida Silva",
                    Type = TransactionType.Transfer,
                    CreatedAt = DateTime.Now.AddDays(-1),
                },
                new()
                {
                    Amount = 91_54,
                    Id = Guid.NewGuid(),
                    Other = "Gilsinho Trembolono",
                    Type = TransactionType.Transfer,
                    CreatedAt = DateTime.Now.AddDays(-2),
                },
                new()
                {
                    Amount = 58_00,
                    Id = Guid.NewGuid(),
                    Other = "Marisvaldison Gomes",
                    Type = TransactionType.Transfer,
                    CreatedAt = DateTime.Now.AddDays(-3),
                },
            }
		);
    }
}
