using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Converters;

namespace PicPay.Shared;

public static class StringExtensions
{
    public static bool IsEmpty(this string? text)
    {
        return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
    }

    public static bool HasValue(this string? text)
    {
        return !string.IsNullOrEmpty(text);
    }

    public static string OnlyNumbers(this string text)
    {
        if (text.HasValue())
        {
            return new string(text.Where(char.IsDigit).ToArray());
        }

        return "";
    }

    public static string ToMoneyFormat(this long? value)
    {
        if (value == null)
            return string.Empty;

        return ToMoneyFormat(value.Value);
    }

    public static string ToMoneyFormat(this long value)
    {
        return string.Format(new CultureInfo("pt-BR", true), "{0:C}", (decimal)value / 100);
    }

	private static JsonSerializerSettings _settings = new()
	{
		Converters = [new StringEnumConverter()],
	};

	public static string Serialize(this object obj)
	{
		return JsonConvert.SerializeObject(obj, _settings);
	}
}
