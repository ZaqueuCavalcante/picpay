namespace PicPay.Api.Settings;

public class NotifySettings
{
    public string Url { get; set; }
    public int Timeout { get; set; }

    public NotifySettings(IConfiguration configuration)
    {
        configuration.GetSection("Notify").Bind(this);
    }
}
