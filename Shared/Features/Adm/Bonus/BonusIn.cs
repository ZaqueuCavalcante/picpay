namespace PicPay.Shared;

public class BonusIn
{
    /// <summary>
    /// Valor do bônus
    /// </summary>
    public long Amount { get; set; }

    /// <summary>
    /// Carteira de destino do bônus
    /// </summary>
    public Guid WalletId { get; set; }

    public BonusIn(
        long amount,
        Guid walletId
    ) {
        Amount = amount;
        WalletId = walletId;
    }
}
