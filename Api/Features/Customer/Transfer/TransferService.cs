using PicPay.Api.Features.Cross.CreateTransaction;

namespace PicPay.Api.Features.Adm.Transfer;

public class TransferService(PicPayDbContext ctx) : IPicPayService
{
    public async Task<OneOf<TransferOut, PicPayError>> Transfer(TransferIn data)
    {
        if (data.Amount <= 0) return new InvalidTransferAmount();

        var wallet = await ctx.Wallets.FirstAsync(w => w.Id == data.WalletId);
        wallet.Put(data.Amount);

        var transaction = new Transaction(wallet.Id, TransactionType.Transfer, data.Amount);
        ctx.Add(transaction);

        await ctx.SaveChangesAsync();

        return new TransferOut { TransactionId = transaction.Id };
    }
}

// - Chamada não autenticada deve receber 403

// - Apenas Clientes podem transferir
//     - Lojista deve receber 401
//     - Adm deve receber 401

// - Não pode transferir valor <= zero
//     - Validar amount enviado

// - Não pode transferir para si próprio
//     - Validar o target e o id do usuário logado

// - Nâo pode transferir pra uma Carteira inexistente
//     - Validar no banco se o target existe

// - Não pode transferir sem saldo suficiente
//     - Validar se tem saldo suficiente

// - Não pode transferir caso seja não autorizado
//     - Caso chamada pro Auth retorne não autorizado, retornar erro

// - Não pode transferir caso o autorizador esteja fora do ar
//     - Caso chamada pro Auth erro/timeout, retornar erro
