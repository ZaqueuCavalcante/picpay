namespace PicPay.Api.Features.Cross.CreateNotification;

[PicPayTaskDescription("Enviar notificação de transferência realizada")]
public record SendTransferNotificationTask(Guid TransactionId) : IPicPayTask;

public class SendTransferNotificationTaskHandler(PicPayDbContext ctx) : IPicPayTaskHandler<SendTransferNotificationTask>
{
    public async Task Handle(SendTransferNotificationTask task)
    {
        var transaction = await ctx.Transactions.AsNoTracking().FirstAsync(x => x.Id == task.TransactionId);
        var notification = await ctx.Notifications.FirstOrDefaultAsync(x => x.TransactionId == task.TransactionId);

        if (notification == null)
        {
            var targetWallet = await ctx.Wallets.AsNoTracking().FirstAsync(x => x.Id == transaction.TargetWalletId);

            var sourceName = await ctx.GetWalletOwnerName(transaction.SourceWalletId);

            notification = Notification.NewTransfer(targetWallet.UserId, transaction.Id, transaction.Amount, sourceName);

            ctx.Add(notification);
            await ctx.SaveChangesAsync();
        }
        else
        {
            notification.Retry();
        }

        // Chamar servico de notificacao
        // Caso de sucesso, marcar como enviada e fim
        // Caso de erro e tenha tentado menos de 3 vezes, enfileirar dnv

        var ok = new Random().NextDouble() > 0.5;
        if (ok)
        {
            notification.Success();
        }
        else
        {
            notification.Fail();
        }

        await ctx.SaveChangesAsync();
    }
}
