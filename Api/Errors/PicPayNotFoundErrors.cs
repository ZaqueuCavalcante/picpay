namespace PicPay.Api.Errors;

public class UserNotFound : PicPayError
{
    public override string Code { get; set; } = nameof(UserNotFound);
    public override string Message { get; set; } = "Usuário não encontrado.";
}

public class WalletNotFound : PicPayError
{
    public override string Code { get; set; } = nameof(WalletNotFound);
    public override string Message { get; set; } = "Carteira não encontrada.";
}
