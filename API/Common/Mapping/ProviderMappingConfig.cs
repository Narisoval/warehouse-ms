using Domain.Entities;
using Warehouse.API.DTO.ProviderDtos;

namespace Warehouse.API.Common.Mapping;

public static class ProviderMappingConfig
{
    public static ProviderDto ToDto(this Provider provider)
    {
        return new ProviderDto
        {
            ProviderId = provider.Id,
            Email = provider.Email.Value,
            PhoneNumber = provider.PhoneNumber,
            CompanyName = provider.CompanyName.Value
        };
    }

    public static ProviderUpdateDto ToUpdateDto(this Provider provider)
    {
        return new ProviderUpdateDto
        {
            CompanyName = provider.CompanyName.Value,
            Email = provider.Email.Value,
            PhoneNumber = provider.PhoneNumber
        };
    }
}