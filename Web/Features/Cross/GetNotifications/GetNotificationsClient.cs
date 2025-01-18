namespace PicPay.Web.Features.Cross.GetNotifications;

public class GetNotificationsClient(HttpClient http) : IPicPayClient
{
    public async Task<List<GetNotificationOut>> Get()
    {
        return await http.GetFromJsonAsync<List<GetNotificationOut>>("/notifications") ?? [];
    }
}
