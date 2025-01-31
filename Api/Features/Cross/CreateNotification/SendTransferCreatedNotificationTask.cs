using PicPay.Api.Features.Cross.Notify;

namespace PicPay.Api.Features.Cross.CreateNotification;

[PicPayTaskDescription("Enviar notificação de Transferência realizada")]
public record SendTransferCreatedNotificationTask(Guid TransactionId) : IPicPayTask;

public class SendTransferNotificationTaskHandler(PicPayDbContext ctx, NotifyService service) : IPicPayTaskHandler<SendTransferCreatedNotificationTask>
{
    public async Task Handle(SendTransferCreatedNotificationTask task)
    {
        var transaction = await ctx.Transactions.AsNoTracking().FirstAsync(x => x.Id == task.TransactionId);

        var targetWallet = await ctx.Wallets.AsNoTracking().FirstAsync(x => x.Id == transaction.TargetWalletId);
        var targetUser = await ctx.Users.AsNoTracking().FirstAsync(x => x.Id == targetWallet.UserId);

        var sourceName = await ctx.GetWalletOwnerName(transaction.SourceWalletId);
        var notification = Notification.NewTransfer(targetWallet.UserId, transaction.Id, transaction.Amount, sourceName);

        ctx.Add(notification);

        var response = await service.Notify(new NotificationIn { Email = targetUser.Email, Message = notification.Message });

        if (response.IsSuccess())
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
