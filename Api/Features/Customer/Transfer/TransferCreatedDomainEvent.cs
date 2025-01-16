using PicPay.Api.Features.Cross.CreateNotification;

namespace PicPay.Api.Features.Adm.Transfer;

[DomainEventDescription("Transferência realizada")]
public record TransferCreatedDomainEvent(Guid TransactionId) : IDomainEvent;

public class TransferCreatedDomainEventHandler(PicPayDbContext ctx) : IDomainEventHandler<TransferCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, TransferCreatedDomainEvent evt)
    {
        await ctx.SaveTasksAsync(eventId, new SendTransferNotificationTask(evt.TransactionId));
    }
}
