namespace Warehouse.API.DTO;

public record BrandDto
{
    public Guid BrandId { get; init; }

    public string Name { get; init; }

    public string Image { get; init; }

    public string Description { get; set; }
    
    public IList<ProductDto>? Products { get; set; }
}
