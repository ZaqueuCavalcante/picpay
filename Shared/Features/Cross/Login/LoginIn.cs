namespace PicPay.Shared;

public class LoginIn
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Senha
    /// </summary>
    public string Password { get; set; }

    public LoginIn(
        string email,
        string password
    ) {
        Email = email;
        Password = password;
    }
}
