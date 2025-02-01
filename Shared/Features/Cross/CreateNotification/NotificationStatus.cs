using System.ComponentModel;

namespace PicPay.Shared;

/// <summary>
/// Status de envio da notificação
/// </summary>
public enum NotificationStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Sucesso")]
    Success,

    [Description("Falha")]
    Failed,
}
