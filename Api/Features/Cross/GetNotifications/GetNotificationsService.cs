namespace PicPay.Api.Features.Cross.GetNotifications;

public class GetNotificationsService(PicPayDbContext ctx) : IPicPayService
{
    public async Task<List<GetNotificationOut>> Get(Guid userId)
    {
        var notifications = await ctx.Notifications.AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return notifications.ConvertAll(n => n.ToOut());
    }
}
