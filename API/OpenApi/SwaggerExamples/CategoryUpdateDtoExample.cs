using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.CategoryDtos;

namespace Warehouse.API.OpenApi.SwaggerExamples;

public class CategoryUpdateDtoExample : IExamplesProvider<CategoryUpdateDto>
{
    public CategoryUpdateDto GetExamples()
    {
        return new CategoryUpdateDto
        {
            Name = "Electronics",
            ParentId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
        };
    }
}