using System.ComponentModel;

namespace PicPay.Shared;

public enum UserRole
{
    [Description("Admin")]
    Adm,

    [Description("Cliente")]
    Customer,

    [Description("Lojista")]
    Merchant,
}
