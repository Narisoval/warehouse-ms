namespace Warehouse.API.DTO.Brand;

public record BrandDto
{
    public Guid BrandId { get; init; }

    public string Name { get; init; }

    public string Image { get; init; }

    public string Description { get; set; }
}
