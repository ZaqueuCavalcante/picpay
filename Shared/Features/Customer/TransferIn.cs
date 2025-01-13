namespace PicPay.Shared;

public class DepositIn
{
    /// <summary>
    /// Valor do depósito
    /// </summary>
    public long Amount { get; set; }

    /// <summary>
    /// Carteira de destino do depósito
    /// </summary>
    public Guid WalletId { get; set; }

    public DepositIn(
        long amount,
        Guid walletId
    ) {
        Amount = amount;
        WalletId = walletId;
    }
}
