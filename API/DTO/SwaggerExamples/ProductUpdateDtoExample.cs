using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.ProductDtos;

namespace Warehouse.API.DTO.SwaggerExamples;

public class ProductUpdateDtoExample : IExamplesProvider<ProductUpdateDto>
{
    public ProductUpdateDto GetExamples()
    {
        Guid exampleId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

        List<string> exampleImages = new List<string>
        { 
            "https://m.media-amazon.com/images/I/81uam2++ZhL._AC_UY500_.jpg",
            "https://m.media-amazon.com/images/I/71+G0a4HNKL._AC_UX395_.jpg",
        };
        
        return new ProductUpdateDto
        {
            Name = "UGG Women's Classic Short Ii Boot",
            Quantity = 3000,
            FullPrice = 169.95M,
            Description =
                "Our Classic Boot was originally worn by surfers to keep warm after early-morning sessions, and has since become iconic for its soft sheepskin and enduring design. Incorporating a durable, lightweight sole to increase cushioning and traction, these versatile boots pair well with practically anything – try loose boyfriend jeans and a velvet top.",
            Images = exampleImages,
            MainImage = "https://m.media-amazon.com/images/I/810o2uaCEKL._AC_UY695_.jpg",
            Sale = 10m,
            IsActive = true,
            CategoryId = exampleId,
            ProviderId = exampleId,
            BrandId = exampleId
        };
    }
}