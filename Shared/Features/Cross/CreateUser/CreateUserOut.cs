namespace PicPay.Shared;

public class CreateUserOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Email { get; set; }
    public Guid WalletId { get; set; }

    public CreateCustomerOut ToCreateCustomerOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Email = Email,
            Cpf = Document,
            WalletId = WalletId,
        };
    }

    public CreateMerchantOut ToCreateMerchantOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Email = Email,
            Cnpj = Document,
            WalletId = WalletId,
        };
    }
}
