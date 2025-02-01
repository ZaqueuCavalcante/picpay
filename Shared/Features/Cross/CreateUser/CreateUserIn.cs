namespace PicPay.Shared;

public class CreateUserIn
{
    /// <summary>
    /// Perfil de acesso do usuário
    /// </summary>
    public UserRole Role { get; set; }

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

    public CreateUserIn(
        UserRole role,
        string name,
        string document,
        string email,
        string password
    ) {
        Role = role;
        Name = name;
        Document = document;
        Email = email;
        Password = password;
    }
}
