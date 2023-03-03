namespace Warehouse.API.DTO.ProductDtos;

public class ProductUpdateDto
{
     public string Name { get; init; } = "";
 
     public int Quantity { get; init; } = default;
 
     public decimal FullPrice { get; init; } = default;
 
     public string Description { get; init; } = "";
 
     public IList<ProductImageDto>? Images { get; init; } = null!;
 
     public decimal Sale { get; init; } = default;
 
     public bool IsActive { get; init; } = default;
 
     public Guid CategoryId { get; init; }
 
     public Guid ProviderId { get; init; }
 
     public Guid BrandId { get; init; }   
}