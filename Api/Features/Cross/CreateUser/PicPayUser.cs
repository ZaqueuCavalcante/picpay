namespace PicPay.Api.Features.Cross.CreateUser;

public class PicPayUser
{
    public Guid Id { get; private set; }
    public UserRole Role { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Wallet Wallet { get; private set; }

    private PicPayUser() { }

    private PicPayUser(
        UserRole role,
        string name,
        string document,
        string email
    ) {
        Id = Guid.NewGuid();
        Role = role;
        Name = name;
        Document = document;
        Email = email;
        CreatedAt = DateTime.Now;

        Wallet = new Wallet(Id, role.ToString().ToEnum<WalletType>());
    }

    public static OneOf<PicPayUser, PicPayError> New(
        UserRole role,
        string name,
        string document,
        string email
    ) {
        if (!document.IsValidDocument()) return new InvalidDocument();

        return new PicPayUser(role, name, document, email);
    }

    public void SetPasswordHash(string passwordHash) => PasswordHash = passwordHash;

    public CreateUserOut ToCreateOut() => new()
    {
        Id = Id,
        Name = Name,
        Document = Document,
        Email = Email,
    };
}
