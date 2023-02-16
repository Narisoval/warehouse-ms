namespace Warehouse.API.DTO.Brand;

public record BrandUpdateDto
{
    public string Name { get; init; } = "";
    
    public string Image { get; init; } = "";
    
    public string Description { get; init; } = "";
}