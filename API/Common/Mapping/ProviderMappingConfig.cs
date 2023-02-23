using Domain.Entities;
using Domain.ValueObjects;
using Warehouse.API.DTO.Bindings;
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

    public static Provider ToEntity(this ProviderDto providerDto)
    {
        return Provider.Create(
            providerDto.ProviderId,
            CompanyName.From(providerDto.CompanyName).Value,
            providerDto.PhoneNumber,
            Email.From(providerDto.Email).Value
        ).Value;
    }

    public static Provider ToEntity(this ProviderUpdateDto providerUpdateDto)
    {
        return Provider.Create(
            CompanyName.From(providerUpdateDto.CompanyName).Value,
            providerUpdateDto.PhoneNumber,
            Email.From(providerUpdateDto.Email).Value
        );
    }

    public static Provider ToEntity(this ProviderUpdateDto providerUpdateDto, Guid id)
    {
        return Provider.Create(
            id,
            CompanyName.From(providerUpdateDto.CompanyName).Value,
            providerUpdateDto.PhoneNumber,
            Email.From(providerUpdateDto.Email).Value
        ).Value;
    }
}