
namespace Warehouse.API.DTO.CategoryDtos;

public record CategoryDto 
{
    public Guid CategoryId { get; set; }
    
    public string Name { get; init; } = "";

    public Guid? ParentId { get; init; } = null!;
    
    public IReadOnlyCollection<CategoryDto>? SubCategories { get; init; }
}
