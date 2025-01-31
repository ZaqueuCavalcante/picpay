namespace PicPay.Api.Features.Adm.Transfer;

[ApiController, AuthCustomer]
[Consumes("application/json"), Produces("application/json")]
public class TransferController(TransferService service) : ControllerBase
{
    /// <summary>
    /// Transferir
    /// </summary>
    /// <remarks>
    /// Cria uma nova Transferência para a Carteira informada.
    /// </remarks>
    [HttpPost("customer/transfers")]
    [ProducesResponseType(typeof(TransferOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Transfer([FromBody] TransferIn data)
    {
        var result = await service.Transfer(User.Id(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IExamplesProvider<TransferIn>
{
    TransferIn IExamplesProvider<TransferIn>.GetExamples()
    {
        return new TransferIn(5_55, Guid.NewGuid());
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<TransferOut>
{
    public IEnumerable<SwaggerExample<TransferOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"TransferOut",
			new TransferOut { TransactionId = Guid.NewGuid() }
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new InvalidTransferAmount().ToExampleErrorOut();
        yield return new InvalidTargetWallet().ToExampleErrorOut();
        yield return new WalletNotFound().ToExampleErrorOut();
        yield return new InsufficientWalletBalance().ToExampleErrorOut();
        yield return new AuthorizeServiceDown().ToExampleErrorOut();
        yield return new TransactionNotAuthorized().ToExampleErrorOut();
    }
}
