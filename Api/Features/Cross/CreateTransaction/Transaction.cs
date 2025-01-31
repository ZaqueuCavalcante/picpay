using PicPay.Api.Features.Adm.Transfer;
using PicPay.Api.Features.Cross.CreateCustomer;

namespace PicPay.Api.Features.Cross.CreateTransaction;

public class Transaction : Entity
{
    public Guid Id { get; private set; }
    public Guid SourceWalletId { get; private set; }
    public Guid TargetWalletId { get; private set; }
    public TransactionType Type { get; private set; }
    public long Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Transaction() { }

    public Transaction(
        Guid sourceWalletId,
        Guid targetWalletId,
        TransactionType type,
        long amount
    ) {
        Id = Guid.NewGuid();
        SourceWalletId = sourceWalletId;
        TargetWalletId = targetWalletId;
        Type = type;
        Amount = amount;
        CreatedAt = DateTime.Now;

        if (type == TransactionType.WelcomeBonus)
        {
            AddDomainEvent(new WelcomeBonusCreatedDomainEvent(Id));
        }

        if (type == TransactionType.Transfer)
        {
            AddDomainEvent(new TransferCreatedDomainEvent(Id));
        }
    }
}
