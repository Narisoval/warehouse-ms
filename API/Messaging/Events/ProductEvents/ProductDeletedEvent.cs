namespace Warehouse.API.Messaging.Events.ProductEvents;

public class ProductDeletedEvent : IEvent
{
    public Guid Id { get; init; }
}