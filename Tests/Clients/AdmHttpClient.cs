namespace PicPay.Tests.Clients;

public class AdmHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public async Task<GetWalletOut> GetWallet()
    {
        return await Http.GetWallet();
    }
}
