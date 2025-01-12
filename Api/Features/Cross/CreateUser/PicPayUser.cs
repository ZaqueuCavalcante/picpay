namespace PicPay.Api.Features.Cross.CreateUser;

public class PicPayUser
{
    public Guid Id { get; private set; }
    public UserType Type { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private PicPayUser() { }

    private PicPayUser(
        UserType type,
        string name,
        string document,
        string email
    ) {
        Id = Guid.NewGuid();
        Type = type;
        Name = name;
        Document = document;
        Email = email;
        CreatedAt = DateTime.Now;
    }

    public static OneOf<PicPayUser, PicPayError> New(
        UserType type,
        string name,
        string document,
        string email
    ) {
        if (!document.IsValidDocument()) return new InvalidDocument();

        return new PicPayUser(type, name, document, email);
    }

    public void SetPasswordHash(string passwordHash) => PasswordHash = passwordHash;

    public CreateUserOut ToCreateOut() => new()
    {
        Id = Id,
        Type = Type,
        Name = Name,
        Document = Document,
        Email = Email,
        CreatedAt = CreatedAt
    };
}
