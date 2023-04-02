using System.Net;
using System.Net.Http.Json;
using API.IntegrationTests.Helpers.Fixtures;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Warehouse.API.DTO.PaginationDtos;
using Warehouse.API.DTO.ProviderDtos;
using Warehouse.API.Helpers.Mapping;

namespace API.IntegrationTests.ControllerTests;

public class ProvidersControllerTests : IClassFixture<WarehouseWebApplicationFactory>
{
    private readonly HttpClient _client;

    private const string Endpoint = "api/providers";

    public ProvidersControllerTests(WarehouseWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    #region GET /all

    [Fact]
    public async Task Should_ReturnAllProviders_When_GetAllEndpointIsCalled()
    {
        // Arrange
        var oneItemResponse = await _client.GetAsync($"{Endpoint}/all?pageIndex=1&pageSize=1");
        var oneItemPageResponse = await oneItemResponse.Content.ReadFromJsonAsync<PageResponse<ProviderDto>>();
        oneItemPageResponse.Should().NotBeNull();
        var pagesSize = oneItemPageResponse!.PaginationInfo.TotalRecords;

        //Act
        var allItemsResponse = await _client.GetAsync($"{Endpoint}/all?pageIndex=1&pageSize={pagesSize}");
        var allItems = await allItemsResponse.Content.ReadFromJsonAsync<PageResponse<ProviderDto>>();

        // Assert
        var seededProvidersDtos = EntitiesFixture.Providers.Select(provider => provider.ToDto());
        allItemsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        oneItemPageResponse.Should().NotBeNull();
        allItems.Should().NotBeNull();
        allItems!.Data.Should().BeEquivalentTo(seededProvidersDtos);
    }

    [Theory]
    [InlineData(1, 2)]
    public async Task Should_ReturnProvidersWithCustomPagination_When_GetAllEndpointIsCalledWithQueryParameters
        (int pageIndex, int pageSize)
    {
        // Act
        var response = await _client.GetAsync($"{Endpoint}/all?pageIndex={pageIndex}&pageSize={pageSize}");
        var result = await response.Content.ReadFromJsonAsync<PageResponse<ProviderDto>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.PaginationInfo.PageIndex.Should().Be(pageIndex);
        result.PaginationInfo.PageSize.Should().Be(pageSize);
        result.PaginationInfo.TotalRecords.Should().Be(EntitiesFixture.Providers.Count);
    }

    [Theory]
    [InlineData(0, 6)]
    [InlineData(6, 0)]
    [InlineData(0, 0)]
    [InlineData(-1, 9)]
    [InlineData(9, -1)]
    [InlineData(-1, -1)]
    public async Task Should_ReturnBadRequest_When_GetAllEndpointIsCalledWithInvalidQueryParameters(int pageIndex,
        int pageSize)
    {
        // Act
        var response = await _client.GetAsync($"{Endpoint}/all?pageIndex={pageIndex}&pageSize={pageSize}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region GET /{id}
    
    [Fact]
    public async Task Should_ReturnProvider_When_GetProviderEndpointIsCalledWithValidId()
    {
        // Arrange
        var expectedProvider = EntitiesFixture.GetRandomEntity<Provider>();
        var providerId = expectedProvider.Id;

        // Act
        var response = await _client.GetAsync($"{Endpoint}/{providerId}");
        var result = await response.Content.ReadFromJsonAsync<ProviderDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedProvider.ToDto());
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_GetEndpointIsCalledWithInvalidId()
    {
        // Arrange
        var invalidCategoryId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"{Endpoint}/{invalidCategoryId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region POST /{id} 

    [Fact]
    public async Task Should_CreateProvider_When_CreateProviderEndpointIsCalledWithValidData()
    {
        // Arrange
        var newProvider = DtosFixture.ProviderUpdateDto;

        // Act
        var response = await _client.PostAsJsonAsync($"{Endpoint}", newProvider);
        var responseFromPost = await response.Content.ReadFromJsonAsync<ProviderDto>();

        var responseFromGet = await _client.GetAsync($"{Endpoint}/{responseFromPost!.ProviderId}");
        var responseFromGetDto = await responseFromGet.Content.ReadFromJsonAsync<ProviderDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        responseFromGet.StatusCode.Should().Be(HttpStatusCode.OK);

        responseFromGetDto.Should().NotBeNull();

        AssertDtosAreEqual(newProvider, responseFromGetDto!);

        responseFromPost.Should().NotBeNull();
        responseFromGetDto.Should().BeEquivalentTo(responseFromPost);

        // Verify 
        await _client.DeleteAsync($"{Endpoint}/{responseFromPost.ProviderId}");
    }
    
    [Fact]
    public async Task Should_Return400_When_CreatingProviderWithInvalidData()
    {
        // Arrange
        var invalidProviderData = DtosFixture.InvalidProviderUpdateDto; 

        // Act
        var response = await _client.PostAsJsonAsync($"{Endpoint}/", invalidProviderData);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);       
    }
    
    #endregion

    #region PUT  

    [Fact]
    public async Task Should_UpdateProvider_When_UpdateProviderEndpointIsCalledWithValidData()
    {
        // Arrange
        var providerToUpdate = EntitiesFixture.GetRandomEntity<Provider>();
        var providerUpdateDto = DtosFixture.ProviderUpdateDto;

        // Act
        var responseFromPut = await _client
            .PutAsJsonAsync($"{Endpoint}/{providerToUpdate.Id}", providerUpdateDto);

        var responseFromGet = await _client.GetAsync($"{Endpoint}/{providerToUpdate.Id}");
        var dtoFromGet = await responseFromGet.Content.ReadFromJsonAsync<ProviderDto>();

        // Assert
        responseFromPut.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseFromGet.StatusCode.Should().Be(HttpStatusCode.OK);

        dtoFromGet.Should().NotBeNull();
        AssertDtosAreEqual(providerUpdateDto, dtoFromGet!);

        //Verify
        await UpdateProviderFromEntity(providerToUpdate);
    }


    [Fact]
    public async Task Should_ReturnBadRequest_When_UpdateProviderEndpointIsCalledWithInvalidData()
    {
        // Arrange
        var providerToUpdate = EntitiesFixture.Providers.First();
        var invalidProviderData = DtosFixture.InvalidProviderUpdateDto;

        // Act
        var response = await _client.PutAsJsonAsync($"{Endpoint}/{providerToUpdate.Id}",
            invalidProviderData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_UpdateProviderEndpointIsCalledWithInvalidId()
    {
        // Arrange
        var invalidProviderId = Guid.NewGuid();
        var updateProvider = DtosFixture.ProviderUpdateDto;

        // Act
        var response = await _client.PutAsJsonAsync($"{Endpoint}/{invalidProviderId}", updateProvider);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    

    #endregion

    #region DELETE /{id} 

    [Fact]
    public async Task Should_DeleteProvider_When_DeleteProviderEndpointIsCalledWithValidId()
    {
        // Arrange
        var createdProviderId = await PostTestProvider();

        // Act
        var response = await _client.DeleteAsync($"{Endpoint}/{createdProviderId}");
        var providerFromGetResponse = await _client.GetAsync($"{Endpoint}/{createdProviderId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        providerFromGetResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_DeleteProviderEndpointIsCalledWithInvalidId()
    {
        // Arrange
        var invalidProviderId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"{Endpoint}/{invalidProviderId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    

    #endregion

    #region Helpers
    
    private async Task<Guid> PostTestProvider()
    {
        var providerToCreate = DtosFixture.ProviderUpdateDto;

        var createProviderResponse = await _client.PostAsJsonAsync($"{Endpoint}", providerToCreate);
        var createdProvider = await createProviderResponse.Content.ReadFromJsonAsync<ProviderDto>();
        return createdProvider!.ProviderId;
    }

    private void AssertDtosAreEqual(ProviderUpdateDto updateDto, ProviderDto providerDto)
    {
        updateDto!.CompanyName.Should().BeEquivalentTo(providerDto.CompanyName);
        updateDto.PhoneNumber.Should().BeEquivalentTo(providerDto.PhoneNumber);
        updateDto.Email.Should().BeEquivalentTo(providerDto.Email);
    }

    private async Task UpdateProviderFromEntity(Provider entity)
    {
        var providerUpdateDto = new ProviderUpdateDto
        {
            CompanyName = entity.CompanyName.Value,
            Email = entity.Email.Value,
            PhoneNumber = entity.PhoneNumber.Value
        };
        await _client.PutAsJsonAsync($"{Endpoint}/{entity.Id}", providerUpdateDto);
    }

    #endregion
}