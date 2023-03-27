#pragma warning disable CS8618
namespace Warehouse.API.Messaging.Events.BrandEvents;

public abstract record BrandEventBase : IEvent
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
    
    public string Image { get; init; }

    public string Description { get; init; }
};