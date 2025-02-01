namespace PicPay.Shared;

public class GetExtractOut
{
    public Guid Id { get; set; }
    public long Amount { get; set; }
    public TransactionType Type { get; set; }
    public string Other { get; set; }
    public DateTime CreatedAt { get; set; }
}
