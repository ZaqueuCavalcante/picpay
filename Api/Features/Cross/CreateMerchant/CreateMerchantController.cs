namespace PicPay.Api.Features.Cross.CreateMerchant;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class CreateMerchantController(CreateMerchantService service) : ControllerBase
{
    /// <summary>
    /// 🔓 Registrar Lojista
    /// </summary>
    /// <remarks>
    /// Cria um novo Lojista.
    /// </remarks>
    [HttpPost("merchants")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateMerchantIn data)
    {
        var result = await service.Create(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IMultipleExamplesProvider<CreateMerchantIn>
{
    public IEnumerable<SwaggerExample<CreateMerchantIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Lojista - Gilbirdelson Lanches",
			new CreateMerchantIn(
                "Gilbirdelson Lanches",
                "55.774.025/0001-34",
                "gilbirdelson.lanches@gmail.com",
                "dc9ab8a5960b44edbcd71ba5ec1a0f")
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
