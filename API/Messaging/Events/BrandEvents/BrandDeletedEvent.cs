namespace Warehouse.API.Messaging.Events.BrandEvents;

public class BrandDeletedEvent : IEvent
{
    public Guid Id { get; init; }
}