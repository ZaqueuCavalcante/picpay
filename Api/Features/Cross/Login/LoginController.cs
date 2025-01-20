namespace PicPay.Api.Features.Cross.Login;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class LoginController(LoginService service) : ControllerBase
{
    /// <summary>
    /// 🔓 Login
    /// </summary>
    /// <remarks>
    /// Realiza o login no sistema.
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] LoginIn data)
    {
        var result = await service.Create(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IMultipleExamplesProvider<LoginIn>
{
    public IEnumerable<SwaggerExample<LoginIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Cliente - João da Silva",
			new LoginIn(
                "joao.da.silva@gmail.com",
                "bfD43ae@8c46cb9fd18")
		);
        yield return SwaggerExample.Create(
			"Lojista - Gilbirdelson Lanches",
			new LoginIn(
                "gilbirdelson.lanches@gmail.com",
                "dc9ab8a59@60b44edbcd71ba5Ec1a0f")
		);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<LoginOut>
{
    public IEnumerable<SwaggerExample<LoginOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"LoginOut",
			new LoginOut
			{
				AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI0N2E1NmJmNC0wZWQwLTQ1NTMtOTBkOS02NTA4OGRkMzNmZGUiLCJzdWIiOiJhNGNiNzk3NC1kOWU5LTRkZDQtOGZhYi1jZGZhZmI3YjMzNTMiLCJyb2xlIjoiQ3VzdG9tZXIiLCJuYW1lIjoiSm_Do28gZGEgU2lsdmEiLCJlbWFpbCI6ImpvYW8uZGEuc2lsdmFAZ21haWwuY29tIiwibmJmIjoxNzM2NzY1MDkxLCJleHAiOjE3MzcxMjUwOTEsImlhdCI6MTczNjc2NTA5MSwiaXNzIjoicGljcGF5LWFwaS1kZXYiLCJhdWQiOiJwaWNwYXktYXBpLWRldiJ9.5DaqVrxMNeM1y3itKa2BlAGHlvrjuBg18rx3NDP9ssg"
			}
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new UserNotFound().ToExampleErrorOut();
        yield return new WrongEmailOrPassword().ToExampleErrorOut();
    }
}
