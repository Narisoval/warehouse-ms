namespace Warehouse.API.Messaging.Events.CategoryEvents;

public class CategoryDeletedEvent : IEvent
{
    public Guid Id { get; init; }
}
