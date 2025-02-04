namespace PicPay.Api.Features.Cross.CreateMerchant;

[ApiController]
[EnableRateLimiting(nameof(RateLimiterSettings.SuperVerySmall))]
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
    [ProducesResponseType(typeof(CreateMerchantOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateMerchantIn data)
    {
        var result = await service.Create(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IExamplesProvider<CreateMerchantIn>
{
    CreateMerchantIn IExamplesProvider<CreateMerchantIn>.GetExamples()
    {
        return new CreateMerchantIn(
            "Gilbirdelson Lanches",
            "55.774.025/0001-34",
            "gilbirdelson.lanches@gmail.com",
            "dc9ab8a59@60b44edbcd71ba5Ec1a0f");
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<CreateMerchantOut>
{
    public IEnumerable<SwaggerExample<CreateMerchantOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"CreateMerchantOut",
			new CreateMerchantOut
			{
				Id = Guid.NewGuid(),
                Name = "Gilbirdelson Lanches",
                Cnpj = "55.774.025/0001-34",
                Email = "gilbirdelson.lanches@gmail.com",
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
