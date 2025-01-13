using System.ComponentModel;

namespace PicPay.Shared;

public enum TransactionType
{
    [Description("Depósito")]
    Deposit,

    [Description("Transferência")]
    Transfer,
}
