using System.ComponentModel;

namespace PicPay.Shared;

/// <summary>
/// Tipo de documento
/// </summary>
public enum DocType
{
    [Description("CPF")]
    CPF,

    [Description("CNPJ")]
    CNPJ,
}
