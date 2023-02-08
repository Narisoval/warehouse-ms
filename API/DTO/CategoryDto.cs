namespace Warehouse.API.DTO;

public record CategoryDto
{
    public Guid CategoryId { get; init; }

    public string Name { get; init; } = "";

    public IList<ProductDto>? Products { get; set; }
}
