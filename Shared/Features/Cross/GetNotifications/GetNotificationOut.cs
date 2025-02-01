namespace PicPay.Shared;

public class GetNotificationOut
{
    /// <summary>
    /// Id da notificação
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Mensagem
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public NotificationStatus Status { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
