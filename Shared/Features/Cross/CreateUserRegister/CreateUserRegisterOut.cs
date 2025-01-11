namespace PicPay.Shared;

public class CreateUserRegisterOut
{
    public Guid Id { get; set; }
    public UserType Type { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
