using System.ComponentModel;

namespace PicPay.Shared;

/// <summary>
/// Tipo do usuário
/// </summary>
public enum UserType
{
    [Description("Cliente")]
    Customer,

    [Description("Lojista")]
    Merchant,
}
