namespace Warehouse.API.Messaging.Events.ProductEvents;

public record ProductDeletedEvent(Guid Id) : IEvent;