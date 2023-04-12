namespace Warehouse.API.DTO.CategoryDtos;

public class CategoryUpdateDto
{
    public string Name { get; init; } = null!;

    public Guid? ParentId { get; init; }
}