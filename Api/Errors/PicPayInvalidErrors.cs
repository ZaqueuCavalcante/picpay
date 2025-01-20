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

public class InvalidBonusAmount : PicPayError
{
    public override string Code { get; set; } = nameof(InvalidBonusAmount);
    public override string Message { get; set; } = "O valor do bônus deve ser maior que zero.";
}

public class InvalidTransferAmount : PicPayError
{
    public override string Code { get; set; } = nameof(InvalidTransferAmount);
    public override string Message { get; set; } = "O valor da transferência deve ser maior que zero.";
}

public class InvalidTargetWallet : PicPayError
{
    public override string Code { get; set; } = nameof(InvalidTargetWallet);
    public override string Message { get; set; } = "Carteira de destino inválida.";
}
