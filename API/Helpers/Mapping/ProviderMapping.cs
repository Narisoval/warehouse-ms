using Domain.Entities;
using Warehouse.API.DTO.ProviderDtos;

namespace Warehouse.API.Helpers.Mapping;

public static class ProviderMapping
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