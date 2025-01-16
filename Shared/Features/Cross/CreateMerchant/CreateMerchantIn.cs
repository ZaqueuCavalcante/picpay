namespace PicPay.Shared;

public class CreateMerchantIn
{
    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// CNPJ
    /// </summary>
    public string Cnpj { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Senha
    /// </summary>
    public string Password { get; set; }

    public CreateMerchantIn(
        string name,
        string cnpj,
        string email,
        string password
    ) {
        Name = name;
        Cnpj = cnpj;
        Email = email;
        Password = password;
    }

    public CreateUserIn ToCreateUserIn()
    {
        return new CreateUserIn(UserRole.Merchant, Name, Cnpj, Email, Password);
    }
}
