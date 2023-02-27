namespace Warehouse.API.DTO.BrandDtos;

public record BrandDto
{
    public Guid BrandId { get; set; }

    public string Name { get; init; }

    public string Image { get; init; }

    public string Description { get; set; }
}
