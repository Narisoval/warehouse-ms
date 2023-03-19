namespace Warehouse.API.Messaging.Events.ProductEvents;

public abstract record ProductEventBase
{
     public Guid Id { get; init; }
     
     public string Name { get; init; }
     
     public string Description { get; init; }
     
     public string MainImage { get; init; }
     
     public List<string>? Images { get; init; }
     
     public decimal FullPrice { get; init; }
 
     public decimal Discount { get; init; }
     
     public int Quantity { get; init; }
     
     public bool IsActive { get; init; }
     
     public Guid CategoryId { get; init; }
     
     public Guid BrandId { get; init; }   
}