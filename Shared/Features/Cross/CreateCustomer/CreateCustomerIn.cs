namespace PicPay.Shared;

public class CreateCustomerIn
{
    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// CPF
    /// </summary>
    public string Cpf { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Senha
    /// </summary>
    public string Password { get; set; }

    public CreateCustomerIn(
        string name,
        string cpf,
        string email,
        string password
    ) {
        Name = name;
        Cpf = cpf;
        Email = email;
        Password = password;
    }

    public CreateUserIn ToCreateUserIn()
    {
        return new CreateUserIn(UserRole.Customer, Name, Cpf, Email, Password);
    }
}
