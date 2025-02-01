using System.ComponentModel;

namespace PicPay.Shared;

/// <summary>
/// Perfil de acesso do usuário
/// </summary>
public enum UserRole
{
    [Description("Admin")]
    Adm,

    [Description("Cliente")]
    Customer,

    [Description("Lojista")]
    Merchant,
}
