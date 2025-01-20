namespace PicPay.Api.Features.Adm.Bonus;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class BonusController(BonusService service) : ControllerBase
{
    /// <summary>
    /// Bônus
    /// </summary>
    /// <remarks>
    /// Cria um novo Bônus para a Carteira informada.
    /// </remarks>
    [HttpPost("adm/bonus")]
    [ProducesResponseType(typeof(BonusOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Bonus([FromBody] BonusIn data)
    {
        var result = await service.Bonus(User.Id(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IExamplesProvider<BonusIn>
{
    BonusIn IExamplesProvider<BonusIn>.GetExamples()
    {
        return new BonusIn(789_42, Guid.NewGuid());
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<BonusOut>
{
    public IEnumerable<SwaggerExample<BonusOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"BonusOut",
			new BonusOut { TransactionId = Guid.NewGuid() }
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new InvalidBonusAmount().ToExampleErrorOut();
    }
}
