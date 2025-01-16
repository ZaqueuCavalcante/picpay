using PicPay.Api.Features.Cross.Authorize;
using PicPay.Api.Features.Cross.CreateTransaction;

namespace PicPay.Api.Features.Adm.Transfer;

public class TransferService(PicPayDbContext ctx, AuthorizeService service) : IPicPayService
{
    public async Task<OneOf<TransferOut, PicPayError>> Transfer(Guid userId, TransferIn data)
    {
        if (data.Amount <= 0) return new InvalidTransferAmount();

        await ctx.Database.BeginTransactionAsync();

        var wallets = await ctx.Wallets.FromSql($"SELECT * FROM picpay.wallets WHERE user_id = {userId} OR id = {data.WalletId} FOR UPDATE").ToListAsync();

        var sourceWallet = wallets.First(w => w.UserId == userId);
        if (data.WalletId == sourceWallet.Id) return new InvalidTargetTransferWallet();

        var targetWallet = wallets.FirstOrDefault(w => w.Id == data.WalletId);
        if (targetWallet == null) return new WalletNotFound();

        if (targetWallet.Type == WalletType.Adm) return new InvalidTargetTransferWallet();

        if (sourceWallet.Balance < data.Amount) return new InsufficientWalletBalance();

        var response = await service.Authorize(data.Amount);
        if (response.IsError()) return response.GetError();

        sourceWallet.Take(data.Amount);
        targetWallet.Put(data.Amount);

        var transaction = new Transaction(sourceWallet.Id, targetWallet.Id, TransactionType.Transfer, data.Amount);
        ctx.Add(transaction);

        await ctx.SaveChangesAsync();

        await ctx.Database.CommitTransactionAsync();

        return new TransferOut { TransactionId = transaction.Id };
    }
}
