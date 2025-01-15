namespace PicPay.Api.Settings;

public class AuthorizeSettings
{
    public string Url { get; set; }
    public int Timeout { get; set; }

    public AuthorizeSettings(IConfiguration configuration)
    {
        configuration.GetSection("Authorize").Bind(this);
    }
}
