namespace PicPay.Api.Settings;

public class NotifySettings
{
    public string Url { get; set; }
    public int Timeout { get; set; }
    public int Delay { get; set; }
    public int MaxRetryAttempts { get; set; }

    public NotifySettings(IConfiguration configuration)
    {
        configuration.GetSection("Notify").Bind(this);
    }
}
