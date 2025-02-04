namespace PicPay.Api.Features.Cross.CreateCustomer;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class CreateCustomerController(CreateCustomerService service) : ControllerBase
{
    /// <summary>
    /// 🔓 Registrar Cliente
    /// </summary>
    /// <remarks>
    /// Cria um novo Cliente.
    /// </remarks>
    [HttpPost("customers")]
    [ProducesResponseType(typeof(CreateCustomerOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateCustomerIn data)
    {
        var result = await service.Create(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IExamplesProvider<CreateCustomerIn>
{
    CreateCustomerIn IExamplesProvider<CreateCustomerIn>.GetExamples()
    {
        return new CreateCustomerIn(
            "João da Silva",
            "084.128.108-48",
            "joao.da.silva@gmail.com",
            "bfD43ae@8c46cb9fd18");
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<CreateCustomerOut>
{
    public IEnumerable<SwaggerExample<CreateCustomerOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"CreateCustomerOut",
			new CreateCustomerOut
			{
				Id = Guid.NewGuid(),
				Name = "João da Silva",
                Cpf = "084.128.108-48",
				Email = "joao.da.silva@gmail.com",
                WalletId = Guid.NewGuid(),
			}
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new InvalidDocument().ToExampleErrorOut();
        yield return new DocumentAlreadyUsed().ToExampleErrorOut();
        yield return new InvalidEmail().ToExampleErrorOut();
        yield return new EmailAlreadyUsed().ToExampleErrorOut();
        yield return new WeakPassword().ToExampleErrorOut();
    }
}
