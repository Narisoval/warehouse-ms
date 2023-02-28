namespace Warehouse.API.DTO.Category;

public record CategoryDto
{
    public Guid CategoryId { get; set; }

    public string Name { get; init; } = "";
}
