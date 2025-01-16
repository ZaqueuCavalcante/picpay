namespace PicPay.Api.Events;

[AttributeUsage(AttributeTargets.Class)]
public class DomainEventDescriptionAttribute(string description) : Attribute
{
    public string Description { get; set; } = description;
}
