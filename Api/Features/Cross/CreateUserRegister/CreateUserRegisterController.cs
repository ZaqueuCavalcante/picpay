namespace PicPay.Api.Features.Cross.CreateUserRegister;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class CreateUserRegisterController(CreateUserRegisterService service) : ControllerBase
{
    /// <summary>
    /// Registrar usuário
    /// </summary>
    /// <remarks>
    /// Cria um novo usuário.
    /// Ele pode ser do tipo Cliente ou Lojista.
    /// </remarks>
    [HttpPost("users")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateUserRegisterIn data)
    {
        var user = await service.Create(data);

        return Ok(user);
    }
}
