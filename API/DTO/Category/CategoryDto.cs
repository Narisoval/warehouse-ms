namespace Warehouse.API.DTO.Category;

public record CategoryDto 
{
    public Guid Id { get; set; }
    public string Name { get; init; } = "";
}
