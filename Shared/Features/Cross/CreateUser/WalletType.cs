using System.ComponentModel;

namespace PicPay.Shared;

public enum WalletType
{
    [Description("Admin")]
    Adm,

    [Description("Cliente")]
    Customer,

    [Description("Lojista")]
    Merchant,
}
