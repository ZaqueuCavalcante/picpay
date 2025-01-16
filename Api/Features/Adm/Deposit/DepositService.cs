using PicPay.Api.Features.Cross.CreateTransaction;

namespace PicPay.Api.Features.Adm.Deposit;

public class DepositService(PicPayDbContext ctx) : IPicPayService
{
    public async Task<OneOf<DepositOut, PicPayError>> Deposit(Guid userId, DepositIn data)
    {
        if (data.Amount <= 0) return new InvalidDepositAmount();

        await ctx.Database.BeginTransactionAsync();

        var wallets = await ctx.Wallets.FromSql($"SELECT * FROM picpay.wallets WHERE user_id = {userId} OR id = {data.WalletId} FOR UPDATE").ToListAsync();

        var sourceWallet = wallets.First(w => w.UserId == userId);
        if (data.WalletId == sourceWallet.Id) return new InvalidTargetWallet();

        var targetWallet = wallets.FirstOrDefault(w => w.Id == data.WalletId);
        if (targetWallet == null) return new WalletNotFound();

        sourceWallet.Take(data.Amount);
        targetWallet.Put(data.Amount);

        var transaction = new Transaction(sourceWallet.Id, targetWallet.Id, TransactionType.Transfer, data.Amount);
        ctx.Add(transaction);

        await ctx.SaveChangesAsync();

        await ctx.Database.CommitTransactionAsync();

        return new DepositOut { TransactionId = transaction.Id };
    }
}
