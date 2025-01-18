using PicPay.Api.Features.Cross.Notify;

namespace PicPay.Api.Features.Cross.CreateNotification;

[PicPayTaskDescription("Enviar notificação de transferência realizada")]
public record SendTransferNotificationTask(Guid TransactionId) : IPicPayTask;

public class SendTransferNotificationTaskHandler(PicPayDbContext ctx, NotifyService service) : IPicPayTaskHandler<SendTransferNotificationTask>
{
    public async Task Handle(SendTransferNotificationTask task)
    {
        var transaction = await ctx.Transactions.AsNoTracking().FirstAsync(x => x.Id == task.TransactionId);
        var notification = await ctx.Notifications.FirstOrDefaultAsync(x => x.TransactionId == task.TransactionId);

        var targetWallet = await ctx.Wallets.AsNoTracking().FirstAsync(x => x.Id == transaction.TargetWalletId);
        var targetUser = await ctx.Users.AsNoTracking().FirstAsync(x => x.Id == targetWallet.UserId);

        if (notification == null)
        {
            var sourceName = await ctx.GetWalletOwnerName(transaction.SourceWalletId);

            notification = Notification.NewTransfer(targetWallet.UserId, transaction.Id, transaction.Amount, sourceName);

            ctx.Add(notification);
            await ctx.SaveChangesAsync();
        }
        else
        {
            notification.Retry();
        }

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
