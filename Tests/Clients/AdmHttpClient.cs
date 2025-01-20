using PicPay.Web.Features.Cross.Bonus;

namespace PicPay.Tests.Clients;

public class AdmHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public async Task<OneOf<BonusOut, ErrorOut>> Bonus(long amount, Guid walletId)
    {
        var client = new BonusClient(Http);
        return await client.Bonus(amount, walletId);
    }

    public async Task<GetWalletOut> GetWallet()
    {
        return await Http.GetWallet();
    }
}
