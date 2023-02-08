namespace Warehouse.API.DTO;

public record ProductDto
{
    public Guid ProductId { get; init; } = default;

    public string Name { get; init; } = "";

    public int Quantity { get; init; } = default;

    public decimal FullPrice { get; init; } = default;

    public string Description { get; init; } = "";

    public IList<ProductImageDto>? Images { get; init; } = null!;

    public decimal Sale { get; init; } = default;


    public bool IsActive { get; init; } = default;

    public CategoryDto? Category { get; init; }
    
    public ProviderDto? ProviderDto { get; init; }

    public BrandDto? Brand { get; init; }
}
