using System.ComponentModel;

namespace PicPay.Shared;

/// <summary>
/// Tipo da transação
/// </summary>
public enum TransactionType
{
    [Description("Bônus de Boas-Vindas")]
    WelcomeBonus,

    [Description("Transferência")]
    Transfer,
}
