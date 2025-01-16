namespace PicPay.Api.Tasks;

[AttributeUsage(AttributeTargets.Class)]
public class PicPayTaskDescriptionAttribute(string description) : Attribute
{
    public string Description { get; set; } = description;
}
