namespace PicPay.Api.Errors;

public class InvalidEmail : PicPayError
{
    public override string Code { get; set; } = nameof(InvalidEmail);
    public override string Message { get; set; } = "Email inválido.";
}

public class InvalidDocument : PicPayError
{
    public override string Code { get; set; } = nameof(InvalidDocument);
    public override string Message { get; set; } = "Documento inválido.";
}
