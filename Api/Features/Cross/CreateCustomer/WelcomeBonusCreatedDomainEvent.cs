namespace PicPay.Api.Features.Cross.CreateCustomer;

[DomainEventDescription("Bônus de Boas-Vindas realizado")]
public record WelcomeBonusCreatedDomainEvent(Guid TransactionId) : IDomainEvent;

public class WelcomeBonusCreatedDomainEventHandler(PicPayDbContext ctx) : IDomainEventHandler<WelcomeBonusCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, WelcomeBonusCreatedDomainEvent evt)
    {
        await ctx.SaveTasksAsync(eventId, new SendWelcomeBonusCreatedNotificationTask(evt.TransactionId));
    }
}
