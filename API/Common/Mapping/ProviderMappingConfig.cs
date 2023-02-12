using Domain.Entities;
using Domain.ValueObjects;
using Warehouse.API.DTO;

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

   public static Provider ToEntity(this ProviderDto providerDto)
   {
      return Provider.Create(
         providerDto.ProviderId,
         CompanyName.From(providerDto.CompanyName),
         providerDto.PhoneNumber,
         Email.From(providerDto.Email)
      );
   }
}