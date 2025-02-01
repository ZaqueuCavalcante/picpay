namespace PicPay.Tests.Clients;

public class MerchantHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public Guid WalletId { get; set; }

    public async Task<GetWalletOut> GetWallet()
    {
        return await Http.GetWallet();
    }

    public async Task<List<GetNotificationOut>> GetNotifications()
    {
        return await Http.GetNotifications();
    }

    public async Task<List<GetExtractOut>> GetExtract()
    {
        return await Http.GetExtract();
    }
}
