namespace PicPay.Shared;

public class TransferIn
{
    /// <summary>
    /// Valor da transferência
    /// </summary>
    public long Amount { get; set; }

    /// <summary>
    /// Carteira de destino da transferência
    /// </summary>
    public Guid WalletId { get; set; }

    public TransferIn(
        long amount,
        Guid walletId
    ) {
        Amount = amount;
        WalletId = walletId;
    }
}
