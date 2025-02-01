namespace PicPay.Shared;

public class GetWalletOut
{
    /// <summary>
    /// Id da carteira
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Saldo da carteira
    /// </summary>
    public long Balance { get; set; }
}
