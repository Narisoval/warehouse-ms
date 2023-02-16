namespace Warehouse.API.DTO.Product;

public record ProductImageDto
{
    public string Image { get; init; } = "";

    public bool IsMain { get; init; }
}