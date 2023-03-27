#pragma warning disable CS8618
namespace Warehouse.API.Messaging.Events.CategoryEvents;

public abstract record CategoryEventBase : IEvent
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
}