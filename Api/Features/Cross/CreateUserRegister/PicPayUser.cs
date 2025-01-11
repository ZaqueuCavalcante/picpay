namespace PicPay.Api.Features.Cross.CreateUserRegister;

public class PicPayUser
{
    public Guid Id { get; private set; }
    public UserType Type { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public PicPayUser() { }

    public PicPayUser(
        UserType type,
        string name,
        string document,
        string email,
        string password
    ) {
        Id = Guid.NewGuid();
        Type = type;
        Name = name;
        Document = document;
        Email = email;
        Password = password;
        CreatedAt = DateTime.Now;
    }

    public CreateUserRegisterOut ToCreateOut() => new()
    {
        Id = Id,
        Type = Type,
        Name = Name,
        Document = Document,
        Email = Email,
        CreatedAt = CreatedAt
    };
}
