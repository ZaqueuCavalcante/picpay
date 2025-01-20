using System.ComponentModel;

namespace PicPay.Shared;

public enum TransactionType
{
    [Description("Bônus")]
    Bonus,

    [Description("Transferência")]
    Transfer,
}
