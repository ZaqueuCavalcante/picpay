namespace PicPay.Shared;

public class CreateCustomerOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public Guid WalletId { get; set; }

    public static implicit operator CreateCustomerOut(OneOf<CreateCustomerOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
