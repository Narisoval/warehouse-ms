namespace Warehouse.API.DTO.CategoryDtos;

public record CategoryDto 
{
    public Guid CategoryId { get; set; }
    public string Name { get; init; } = "";
}
