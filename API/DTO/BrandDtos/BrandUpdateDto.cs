namespace Warehouse.API.DTO.BrandDtos;

public record BrandUpdateDto
{
    public string Name { get; init; } = "";
    
    public string Image { get; init; } = "";
    
    public string Description { get; init; } = "";
}