using PicPay.Web.Features.Cross.Transfer;
using PicPay.Web.Features.Cross.GetSignUpBonus;

namespace PicPay.Tests.Clients;

public class CustomerHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public async Task<OneOf<SuccessOut, ErrorOut>> GetSignUpBonus()
    {
        var client = new GetSignUpBonusClient(Http);
        return await client.GetSignUpBonus();
    }

    public async Task<OneOf<TransferOut, ErrorOut>> Transfer(long amount, Guid walletId)
    {
        var client = new TransferClient(Http);
        return await client.Transfer(amount, walletId);
    }

    public async Task<GetWalletOut> GetWallet()
    {
        return await Http.GetWallet();
    }
}
