namespace PicPay.Api.Features.Cross.CreateUser;

public class Wallet
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public WalletType Type { get; private set; }
    public long Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Wallet() { }

    public Wallet(
        Guid userId,
        WalletType type
    ) {
        Id = Guid.NewGuid();
        UserId = userId;
        Type = type;
        Balance = 0;
        CreatedAt = DateTime.Now;
    }
}
