namespace PicPay.Shared;

public class CreateMerchantOut
{
    /// <summary>
    /// Id do usuário criado
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// CNPJ
    /// </summary>
    public string Cnpj { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Id da carteira do usuário
    /// </summary>
    public Guid WalletId { get; set; }

    public static implicit operator CreateMerchantOut(OneOf<CreateMerchantOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
