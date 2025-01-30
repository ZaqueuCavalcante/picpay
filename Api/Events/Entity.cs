namespace PicPay.Api.Events;

public abstract class Entity
{
    private List<IDomainEvent> _events = [];

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _events.Clear();
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return [.. _events];
    }
}
