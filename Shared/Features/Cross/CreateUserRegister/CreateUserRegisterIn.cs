namespace PicPay.Shared;

public class CreateUserRegisterIn
{
    public UserType Type { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
