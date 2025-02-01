using System.ComponentModel;

namespace PicPay.Shared;

public enum TransactionType
{
    /// <summary>
    /// Bônus de Boas-Vindas
    /// </summary>
    [Description("Bônus de Boas-Vindas")]
    WelcomeBonus,

    /// <summary>
    /// Transferência
    /// </summary>
    [Description("Transferência")]
    Transfer,
}
