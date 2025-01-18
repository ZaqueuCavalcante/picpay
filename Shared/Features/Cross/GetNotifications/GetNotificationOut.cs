namespace PicPay.Shared;

public class GetNotificationOut
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public NotificationStatus Status { get; set; }
}
