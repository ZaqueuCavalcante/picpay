using System.ComponentModel;

namespace PicPay.Shared;

public enum DocType
{
    [Description("CPF")]
    CPF,

    [Description("CNPJ")]
    CNPJ,
}
