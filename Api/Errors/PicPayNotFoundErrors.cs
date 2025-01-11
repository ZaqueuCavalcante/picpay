namespace PicPay.Api.Errors;

public class WalletNotFound : PicPayError
{
    public override string Code { get; set; } = nameof(WalletNotFound);
    public override string Message { get; set; } = "Carteira não encontrada.";
}
