using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.Category;

namespace Warehouse.API.DTO.SwaggerExamples;

public class CategoryUpdateDtoExample : IExamplesProvider<CategoryUpdateDto>
{
    public CategoryUpdateDto GetExamples()
    {
        return new CategoryUpdateDto
        {
            Name = "Electronics"
        };
    }
}