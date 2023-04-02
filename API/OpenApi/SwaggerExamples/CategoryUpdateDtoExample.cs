using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.CategoryDtos;

namespace Warehouse.API.OpenApi.SwaggerExamples;

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