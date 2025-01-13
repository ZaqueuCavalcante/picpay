namespace PicPay.Shared;

public class CreateUserOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Email { get; set; }

    public CreateCustomerOut ToCreateCustomerOut()
    {
        return new CreateCustomerOut
        {
            Id = Id,
            Name = Name,
            Cpf = Document,
            Email = Email,
        };
    }

    public CreateMerchantOut ToCreateMerchantOut()
    {
        return new CreateMerchantOut
        {
            Id = Id,
            Name = Name,
            Cnpj = Document,
            Email = Email,
        };
    }
}
