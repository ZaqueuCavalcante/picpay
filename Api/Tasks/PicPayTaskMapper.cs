namespace PicPay.Api.Tasks;

public static class PicPayTaskMapper
{
    public static string ToPicPayTaskDescription(this string value)
    {
        if (value.IsEmpty()) return value;

        var type = typeof(IPicPayTask).Assembly.GetType(value)!;
        var customAttributes = (PicPayTaskDescriptionAttribute[])type.GetCustomAttributes(typeof(PicPayTaskDescriptionAttribute), true);

        return customAttributes[0].Description;
    }
}
