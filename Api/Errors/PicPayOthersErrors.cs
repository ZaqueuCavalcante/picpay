namespace PicPay.Api.Errors;

public class DocumentAlreadyUsed : PicPayError
{
    public override string Code { get; set; } = nameof(DocumentAlreadyUsed);
    public override string Message { get; set; } = "Documento já utilizado.";
}

public class EmailAlreadyUsed : PicPayError
{
    public override string Code { get; set; } = nameof(EmailAlreadyUsed);
    public override string Message { get; set; } = "Email já utilizado.";
}

public class WeakPassword : PicPayError
{
    public override string Code { get; set; } = nameof(WeakPassword);
    public override string Message { get; set; } = "Senha fraca.";
}

public class WrongEmailOrPassword : PicPayError
{
    public override string Code { get; set; } = nameof(WrongEmailOrPassword);
    public override string Message { get; set; } = "Email ou senha incorretos.";
}

public class InsufficientWalletBalance : PicPayError
{
    public override string Code { get; set; } = nameof(InsufficientWalletBalance);
    public override string Message { get; set; } = "Saldo insuficiente.";
}

public class AuthorizeServiceDown : PicPayError
{
    public override string Code { get; set; } = nameof(AuthorizeServiceDown);
    public override string Message { get; set; } = "Autorizador fora do ar.";
}

public class TransactionNotAuthorized : PicPayError
{
    public override string Code { get; set; } = nameof(TransactionNotAuthorized);
    public override string Message { get; set; } = "Transação não autorizada.";
}
