namespace PicPay.Shared;

public class CreateUserOut
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
    /// Documento
    /// </summary>
    public string Document { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Id da carteira do usuário
    /// </summary>
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
