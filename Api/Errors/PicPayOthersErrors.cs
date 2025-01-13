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
