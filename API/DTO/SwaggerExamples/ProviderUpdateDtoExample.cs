using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.DTO.ProviderDtos;

namespace Warehouse.API.DTO.SwaggerExamples;

public class ProviderUpdateDtoExample  : IExamplesProvider<ProviderUpdateDto>
{
    public ProviderUpdateDto GetExamples()
    {
        return new ProviderUpdateDto
        {
            CompanyName = "Mars Wrigley Confectionery",
            PhoneNumber = "+380680586459",
            Email = "mars@gmail.com"
        };
    }
}