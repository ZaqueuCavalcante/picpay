using PicPay.Api.Features.Cross.CreateTransaction;

namespace PicPay.Api.Features.Adm.Deposit;

public class DepositService(PicPayDbContext ctx) : IPicPayService
{
    public async Task<OneOf<DepositOut, PicPayError>> Deposit(Guid userId, DepositIn data)
    {
        if (data.Amount <= 0) return new InvalidDepositAmount();

        var sourceWallet = await ctx.Wallets.FirstAsync(w => w.UserId == userId);
        sourceWallet.Take(data.Amount);

        var wallet = await ctx.Wallets.FirstAsync(w => w.Id == data.WalletId);
        wallet.Put(data.Amount);

        var transaction = new Transaction(wallet.Id, TransactionType.Deposit, data.Amount);
        ctx.Add(transaction);

        await ctx.SaveChangesAsync();

        return new DepositOut { TransactionId = transaction.Id };
    }
}
