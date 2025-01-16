namespace PicPay.Web.Features.Cross.GetWallet;

public class GetWalletClient(HttpClient http) : IPicPayClient
{
    public async Task<GetWalletOut> Get()
    {
        return await http.GetFromJsonAsync<GetWalletOut>("/wallet") ?? new();
    }
}
