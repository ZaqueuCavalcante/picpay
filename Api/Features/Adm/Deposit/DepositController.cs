namespace PicPay.Api.Features.Adm.Deposit;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class DepositController(DepositService service) : ControllerBase
{
    /// <summary>
    /// Depositar
    /// </summary>
    /// <remarks>
    /// Cria um novo Depósito para a Carteira informada.
    /// </remarks>
    [HttpPost("adm/deposits")]
    [ProducesResponseType(typeof(DepositOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Deposit([FromBody] DepositIn data)
    {
        var result = await service.Deposit(User.Id(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IExamplesProvider<DepositIn>
{
    DepositIn IExamplesProvider<DepositIn>.GetExamples()
    {
        return new DepositIn(789_42, Guid.NewGuid());
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<DepositOut>
{
    public IEnumerable<SwaggerExample<DepositOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"DepositOut",
			new DepositOut { TransactionId = Guid.NewGuid() }
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new InvalidDepositAmount().ToExampleErrorOut();
    }
}
