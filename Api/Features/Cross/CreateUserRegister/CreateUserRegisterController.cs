using Swashbuckle.AspNetCore.Filters;

namespace PicPay.Api.Features.Cross.CreateUserRegister;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class CreateUserRegisterController(CreateUserRegisterService service) : ControllerBase
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
    public async Task<IActionResult> Create([FromBody] CreateUserRegisterIn data)
    {
        var result = await service.Create(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IMultipleExamplesProvider<CreateUserRegisterIn>
{
    public IEnumerable<SwaggerExample<CreateUserRegisterIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Cliente - João da Silva",
			new CreateUserRegisterIn()
            {
                Type = UserType.Customer,
                Name = "João da Silva",
                Document = "084.128.108-48",
                Email = "joaodasilva@gmail.com",
                Password = "bfD43ae8c46cb9fd18"
            }
		);
        yield return SwaggerExample.Create(
			"Lojista - Gilbirdelson Lanches",
			new CreateUserRegisterIn()
            {
                Type = UserType.Merchant,
                Name = "Gilbirdelson Lanches",
                Document = "55.774.025/0001-34",
                Email = "gilbirdelson.lanches@gmail.com",
                Password = "dc9ab8a5960b44edbcd71ba5ec1a0f"
            }
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new InvalidDocument().ToSwaggerExampleErrorOut();
        yield return new DocumentAlreadyUsed().ToSwaggerExampleErrorOut();
        yield return new InvalidEmail().ToSwaggerExampleErrorOut();
        yield return new EmailAlreadyUsed().ToSwaggerExampleErrorOut();
        yield return new WeakPassword().ToSwaggerExampleErrorOut();
    }
}
