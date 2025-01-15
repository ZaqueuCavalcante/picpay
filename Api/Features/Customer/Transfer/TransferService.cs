using PicPay.Api.Features.Cross.Authorize;
using PicPay.Api.Features.Cross.CreateTransaction;

namespace PicPay.Api.Features.Adm.Transfer;

public class TransferService(PicPayDbContext ctx, AuthorizeService service) : IPicPayService
{
    public async Task<OneOf<TransferOut, PicPayError>> Transfer(Guid userId, TransferIn data)
    {
        if (data.Amount <= 0) return new InvalidTransferAmount();

        var sourceWallet = await ctx.Wallets.FirstAsync(w => w.UserId == userId);
        if (data.WalletId == sourceWallet.Id) return new InvalidTargetWallet();

        var targetWallet = await ctx.Wallets.FirstOrDefaultAsync(w => w.Id == data.WalletId);
        if (targetWallet == null) return new WalletNotFound();

        if (sourceWallet.Balance < data.Amount) return new InsufficientWalletBalance();

        var response = await service.Authorize(data.Amount);
        if (response.IsError()) return response.GetError();

        sourceWallet.Take(data.Amount);
        targetWallet.Put(data.Amount);

        var transaction = new Transaction(targetWallet.Id, TransactionType.Transfer, data.Amount);
        ctx.Add(transaction);

        await ctx.SaveChangesAsync();

        return new TransferOut { TransactionId = transaction.Id };
    }
}



// - Não pode transferir caso seja não autorizado
//     - Caso chamada pro Auth retorne não autorizado, retornar erro

// - Não pode transferir caso o autorizador esteja fora do ar
//     - Caso chamada pro Auth erro/timeout, retornar erro
