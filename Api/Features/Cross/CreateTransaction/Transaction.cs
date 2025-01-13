namespace PicPay.Api.Features.Cross.CreateTransaction;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid WalletId { get; private set; }
    public TransactionType Type { get; private set; }
    public long Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Transaction() { }

    public Transaction(
        Guid walletId,
        TransactionType type,
        long amount
    ) {
        Id = Guid.NewGuid();
        WalletId = walletId;
        Type = type;
        Amount = amount;
        CreatedAt = DateTime.Now;
    }
}
