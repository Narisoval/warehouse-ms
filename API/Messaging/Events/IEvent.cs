namespace Warehouse.API.Messaging.Events;

public interface IEvent
{
    public Guid Id { get; init; } 
}