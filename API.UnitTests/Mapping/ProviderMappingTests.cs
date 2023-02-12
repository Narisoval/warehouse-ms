using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO;

namespace API.UnitTests.Mapping;

public class ProviderMappingTests
{
    private Provider _provider;
    private ProviderDto _providerDto;

    public ProviderMappingTests()
    {
        _provider = ProvidersFixture.GetTestProvider();

        _providerDto = new ProviderDto
        {
            ProviderId = Guid.NewGuid(),
            CompanyName = _provider.CompanyName.Value,
            Email = _provider.Email.Value,
            PhoneNumber = _provider.PhoneNumber
        };
    }
    
    [Fact]
    public void Should_MapProviderToProviderDto_When_ProviderIsValid()
    {
        var mappedProviderDto = _provider.ToDto();
    
        Assert.Equal(_provider.Id, mappedProviderDto.ProviderId);
        Assert.Equal(_provider.CompanyName.Value, mappedProviderDto.CompanyName);
        Assert.Equal(_provider.Email.Value, mappedProviderDto.Email);
        Assert.Equal(_provider.PhoneNumber, mappedProviderDto.PhoneNumber);
    }

    [Fact]
    public void Should_MapProviderDtoToProvider_When_ProviderDtoIsValid()
    {
        var mappedProvider = _providerDto.ToEntity();
    
        Assert.Equal(_providerDto.ProviderId, mappedProvider.Id);
        Assert.Equal(_providerDto.CompanyName, mappedProvider.CompanyName.Value);
        Assert.Equal(_providerDto.PhoneNumber, mappedProvider.PhoneNumber);
        Assert.Equal(_providerDto.Email, mappedProvider.Email.Value);
    }
 
}