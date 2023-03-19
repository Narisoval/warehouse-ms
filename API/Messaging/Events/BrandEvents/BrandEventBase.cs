#pragma warning disable CS8618
namespace Warehouse.API.Messaging.Events.BrandEvents;

public abstract record BrandEventBase
{
    public Guid Id { get; set; }
    
    public string Name { get; init; }
    
    public string Image { get; init; }

    public string Description { get; init; }
};