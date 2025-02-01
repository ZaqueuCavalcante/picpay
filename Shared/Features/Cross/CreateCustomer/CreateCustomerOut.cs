namespace PicPay.Shared;

public class CreateCustomerOut
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
    /// CPF
    /// </summary>
    public string Cpf { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Id da carteira do usuário
    /// </summary>
    public Guid WalletId { get; set; }

    public static implicit operator CreateCustomerOut(OneOf<CreateCustomerOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
