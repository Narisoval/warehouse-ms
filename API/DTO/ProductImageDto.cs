namespace Warehouse.API.DTO;

public record ProductImageDto
{
    public Guid ProductImageId { get; set; }

    public string Image { get; init; } = "";

    public bool IsMain { get; init; }
}