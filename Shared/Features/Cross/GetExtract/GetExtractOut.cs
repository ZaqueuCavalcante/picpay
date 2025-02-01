namespace PicPay.Shared;

public class GetExtractOut
{
    /// <summary>
    /// Id da transação
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Valor da transação em centavos. <br/>
    /// Um valor positivo representa um crédito e um negativo, um débito.
    /// </summary>
    public long Amount { get; set; }

    /// <summary>
    /// Tipo da transação
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Origem ou destino da transação
    /// </summary>
    public string Other { get; set; }

    /// <summary>
    /// Data da transação
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
