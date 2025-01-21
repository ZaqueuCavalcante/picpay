using PicPay.Api.Features.Cross.CreateTransaction;

namespace PicPay.Api.Features.Adm.Transfer.GetSignUpBonus;

public class GetSignUpBonusService(PicPayDbContext ctx) : IPicPayService
{
    public async Task<OneOf<PicPaySuccess, PicPayError>> GetSignUpBonus(Guid userId)
    {
        const long amount = 10_00;

        await ctx.Database.BeginTransactionAsync();

        var admUserId = await ctx.Users.AsNoTracking().Where(x => x.Role == UserRole.Adm).Select(x => x.Id).FirstAsync();

        var wallets = await ctx.Wallets.FromSql($"SELECT * FROM picpay.wallets WHERE user_id = {admUserId} OR user_id = {userId} FOR UPDATE").ToListAsync();

        var sourceWallet = wallets.First(w => w.UserId == admUserId);
        var targetWallet = wallets.First(w => w.UserId == userId);

        var bonusExists = await ctx.Transactions.Where(x => x.SourceWalletId == sourceWallet.Id && x.TargetWalletId == targetWallet.Id).AnyAsync();
        if (bonusExists) return new PicPaySuccess();

        sourceWallet.Take(amount);
        targetWallet.Put(amount);

        var transaction = new Transaction(sourceWallet.Id, targetWallet.Id, TransactionType.Bonus, amount);
        ctx.Add(transaction);

        await ctx.SaveChangesAsync();

        await ctx.Database.CommitTransactionAsync();

        return new PicPaySuccess();
    }
}
