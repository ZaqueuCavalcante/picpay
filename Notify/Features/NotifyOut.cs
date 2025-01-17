namespace PicPay.Notify.Features;

public class NotifyOut
{
    public string Status { get; set; }
    public string Message { get; set; }

    public static NotifyOut NewError()
    {
        return new NotifyOut
        {
            Status = "error",
            Message = "The service is not available, try again later",
        };
    }
}
