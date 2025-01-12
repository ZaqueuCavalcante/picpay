namespace PicPay.Shared;

public class CreateUserIn
{
    public UserType Type { get; set; }

    /// <summary>
    /// Nome completo
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Documento (CPF ou CNPJ)
    /// </summary>
    public string Document { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Senha
    /// </summary>
    public string Password { get; set; }
}
