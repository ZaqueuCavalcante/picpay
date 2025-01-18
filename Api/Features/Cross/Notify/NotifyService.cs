namespace PicPay.Api.Features.Cross.Notify;

public class NotifyService(NotifySettings settings, IHttpClientFactory factory) : IPicPayService
{
    public async Task<OneOf<bool, PicPayError>> Notify(NotificationIn notification)
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri(settings.Url);
        client.Timeout = TimeSpan.FromSeconds(settings.Timeout);

        try
        {
            var response = await client.PostAsJsonAsync($"api/v1/notify", notification);
            if (!response.IsSuccessStatusCode) return new NotifyServiceDown();

            return true;
        }
        catch (Exception)
        {
            return new NotifyServiceDown();
        }
    }
}
