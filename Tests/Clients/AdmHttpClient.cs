using PicPay.Web.Features.Cross.Deposit;

namespace PicPay.Tests.Clients;

public class AdmHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public async Task<OneOf<DepositOut, ErrorOut>> Deposit(long amount, Guid walletId)
    {
        var client = new DepositClient(Http);
        return await client.Deposit(amount, walletId);
    }

    public async Task<GetWalletOut> GetWallet()
    {
        return await Http.GetWallet();
    }
}
