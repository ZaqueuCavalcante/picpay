namespace PicPay.Vendors.Extensions;

public static class StringExtensions
{
    public static string OnlyNumbers(this string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            return new string(text.Where(char.IsDigit).ToArray());
        }

        return "";
    }
}
