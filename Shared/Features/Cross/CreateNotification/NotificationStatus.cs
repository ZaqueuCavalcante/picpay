using System.ComponentModel;

namespace PicPay.Shared;

public enum NotificationStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Sucesso")]
    Success,

    [Description("Falha")]
    Failed,
}
