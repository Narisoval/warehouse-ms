using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.BrandDtos;

namespace Warehouse.API.OpenApi.SwaggerExamples;

public class BrandUpdateDtoExample : IExamplesProvider<BrandUpdateDto>
{
    public BrandUpdateDto GetExamples()
    {
        return new BrandUpdateDto
        {
            Name = "Adidas",
            Description = "Our clothes will be worn by you! Whether you want it or not!",
            Image = "https://pngimg.com/uploads/adidas/adidas_PNG8.png"
        };
    }
}