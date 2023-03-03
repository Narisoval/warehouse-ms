namespace Warehouse.API.DTO.ProductDtos;

public record ProductImageDto
{
    public string Image { get; init; } = "";

    public bool IsMain { get; init; }
}