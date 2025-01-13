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

public class InvalidDepositAmount : PicPayError
{
    public override string Code { get; set; } = nameof(InvalidDepositAmount);
    public override string Message { get; set; } = "O valor do depósito deve ser maior que zero.";
}

public class InvalidTransferAmount : PicPayError
{
    public override string Code { get; set; } = nameof(InvalidTransferAmount);
    public override string Message { get; set; } = "O valor da transferência deve ser maior que zero.";
}
