using System.ComponentModel;

namespace PicPay.Shared;

public enum TransactionType
{
    [Description("Bônus de Boas-Vindas")]
    WelcomeBonus,

    [Description("Transferência")]
    Transfer,
}
