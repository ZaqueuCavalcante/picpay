namespace PicPay.Shared;

public class CreateMerchantOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public string Email { get; set; }
    public Guid WalletId { get; set; }

    public static implicit operator CreateMerchantOut(OneOf<CreateMerchantOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
