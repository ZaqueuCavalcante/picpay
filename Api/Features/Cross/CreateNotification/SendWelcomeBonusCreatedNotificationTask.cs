using PicPay.Api.Features.Cross.Notify;

namespace PicPay.Api.Features.Cross.CreateNotification;

[PicPayTaskDescription("Enviar notificação de Bônus de Boas-Vindas realizado")]
public record SendWelcomeBonusCreatedNotificationTask(Guid TransactionId) : IPicPayTask;

public class SendWelcomeBonusCreatedNotificationTaskHandler(PicPayDbContext ctx, NotifyService service) : IPicPayTaskHandler<SendWelcomeBonusCreatedNotificationTask>
{
    public async Task Handle(SendWelcomeBonusCreatedNotificationTask task)
    {
        var transaction = await ctx.Transactions.AsNoTracking().FirstAsync(x => x.Id == task.TransactionId);

        var targetWallet = await ctx.Wallets.AsNoTracking().FirstAsync(x => x.Id == transaction.TargetWalletId);
        var targetUser = await ctx.Users.AsNoTracking().FirstAsync(x => x.Id == targetWallet.UserId);

        var notification = Notification.NewWelcomeBonus(targetWallet.UserId, transaction.Id, transaction.Amount);

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
