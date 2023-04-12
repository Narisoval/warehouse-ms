
namespace Warehouse.API.DTO.CategoryDtos;

public class CategoryDto 
{
    public Guid CategoryId { get; set; }
    
    public string Name { get; init; } = "";

    public Guid? ParentId { get; init; } 
    
    public IReadOnlyCollection<CategoryDto>? SubCategories { get; init; }
}
