using System.ComponentModel;

namespace PicPay.Shared;

/// <summary>
/// Tipo de carteira
/// </summary>
public enum WalletType
{
    [Description("Admin")]
    Adm,

    [Description("Cliente")]
    Customer,

    [Description("Lojista")]
    Merchant,
}
