namespace PicPay.Api.Features.Cross.CreateNotification;

public class Notification
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid TransactionId { get; private set; }
    public string Message { get; private set; }
    public NotificationStatus Status { get; private set; }
    public int Retries { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Notification() { }

    public Notification(
        Guid userId,
        Guid transactionId,
        string message
    ) {
        Id = Guid.NewGuid();
        UserId = userId;
        TransactionId = transactionId;
        Message = message;
        Status = NotificationStatus.Pending;
        CreatedAt = DateTime.Now;
    }

    public static Notification NewTransfer(
        Guid userId,
        Guid transactionId,
        long amount,
        string sourceName
    ) {
        return new Notification(
            userId,
            transactionId,
            $"Você recebeu uma transferência de {amount.ToMoneyFormat()} de {sourceName}"
        );
    } 

    public void Retry()
    {
        Retries ++;
    }

    public void Success()
    {
        Status = NotificationStatus.Success;
    }

    public void Fail()
    {
        Status = NotificationStatus.Failed;
    }
}
