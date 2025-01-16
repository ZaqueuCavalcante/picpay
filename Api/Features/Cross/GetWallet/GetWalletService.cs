namespace PicPay.Api.Features.Cross.GetWallet;

public class GetWalletService(PicPayDbContext ctx) : IPicPayService
{
    public async Task<GetWalletOut> GetWallet(Guid userId)
    {
        var wallet = await ctx.Wallets.FirstAsync(w => w.UserId == userId);

        return new GetWalletOut { Id = wallet.Id, Balance = wallet.Balance };
    }
}
