using PicPay.Web.Features.Cross.Transfer;

namespace PicPay.Tests.Clients;

public class CustomerHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public string UserName { get; set; }

    public async Task<OneOf<TransferOut, ErrorOut>> Transfer(long amount, Guid walletId)
    {
        var client = new TransferClient(Http);
        return await client.Transfer(amount, walletId);
    }

    public async Task<GetWalletOut> GetWallet()
    {
        return await Http.GetWallet();
    }

    public async Task<List<GetNotificationOut>> GetNotifications()
    {
        return await Http.GetNotifications();
    }
}
