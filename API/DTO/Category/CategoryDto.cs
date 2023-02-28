namespace Warehouse.API.DTO.Category;

public record CategoryDto : IEntityDto
{
    public Guid Id { get; set; }
    public string Name { get; init; } = "";
}
