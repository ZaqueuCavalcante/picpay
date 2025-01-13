using Swashbuckle.AspNetCore.Filters;

namespace PicPay.Api.Features.Cross.CreateUser;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class CreateUserController(CreateUserService service) : ControllerBase
{
    /// <summary>
    /// 🔓 Registrar usuário
    /// </summary>
    /// <remarks>
    /// Cria um novo usuário. <br/>
    /// Ele pode ser do tipo Cliente ou Lojista.
    /// </remarks>
    [HttpPost("users")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateUserIn data)
    {
        var result = await service.Create(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IMultipleExamplesProvider<CreateUserIn>
{
    public IEnumerable<SwaggerExample<CreateUserIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Cliente - João da Silva",
			new CreateUserIn(
                UserType.Customer,
                "João da Silva",
                "084.128.108-48",
                "joaodasilva@gmail.com",
                "bfD43ae8c46cb9fd18")
		);
        yield return SwaggerExample.Create(
			"Lojista - Gilbirdelson Lanches",
			new CreateUserIn(
                UserType.Merchant,
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
