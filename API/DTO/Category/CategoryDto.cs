namespace Warehouse.API.DTO.Category;

public record CategoryDto
{
    public Guid CategoryId { get; init; }

    public string Name { get; init; } = "";
}
