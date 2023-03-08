using Warehouse.API.DTO.BrandDtos;
using Warehouse.API.DTO.ProviderDtos;

namespace Warehouse.API.DTO.ProductDtos;

public record ProductDto 
{
    public Guid ProductId { get; set; }

    public string Name { get; init; } = "";

    public int Quantity { get; init; } 

    public decimal FullPrice { get; init; } 

    public string Description { get; init; } = "";

    public string MainImage { get; init; } = "";
    
    public IReadOnlyCollection<string>? Images { get; init; } 

    public decimal Sale { get; init; }

    public bool IsActive { get; init; }

    public string? Category { get; init; }
    
    public ProviderUpdateDto? Provider { get; init; }

    public BrandUpdateDto? Brand { get; init; }
}
