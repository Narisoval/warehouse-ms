using System.Net;
using System.Net.Http.Json;
using API.IntegrationTests.Helpers;
using API.IntegrationTests.Helpers.Fixtures;
using Domain.Entities;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.API.DTO.BrandDtos;
using Warehouse.API.DTO.PaginationDtos;
using Warehouse.API.Helpers.Mapping;
using Warehouse.API.Messaging.Events.BrandEvents;

namespace API.IntegrationTests.ControllerTests;

public class BrandsControllerTests : IClassFixture<WarehouseWebApplicationFactory>
{
    private readonly HttpClient _client;

    private readonly WarehouseWebApplicationFactory _factory;

    private readonly ControllerTestHelper _testHelper;

    private const string Endpoint = "api/brands";

    public BrandsControllerTests(WarehouseWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var testHarness = _factory.Services.GetRequiredService<ITestHarness>();
        var semaphore = new SemaphoreSlim(1, 1);

        _testHelper = new ControllerTestHelper(semaphore, testHarness);
    }

    #region GET /all 
    
    [Fact]
    public async Task Should_ReturnAllBrands_When_GetAllEndpointIsCalled()
    {
        // Arrange
        var oneItemResponse = await _client.GetAsync($"{Endpoint}/all?pageIndex=1&pageSize=1");
        var oneItemPageResponse = await oneItemResponse.Content.ReadFromJsonAsync<PageResponse<BrandDto>>();
        oneItemPageResponse.Should().NotBeNull();
        var pagesSize = oneItemPageResponse!.PaginationInfo.TotalRecords;

        //Act
        var allItemsResponse = await _client.GetAsync($"{Endpoint}/all?pageIndex={1}&pageSize={pagesSize}");
        var allItems = await allItemsResponse.Content.ReadFromJsonAsync<PageResponse<BrandDto>>();
        var seededBrandsDtos = EntitiesFixture.Brands.Select(brand => brand.ToDto());

        //Assert
        oneItemPageResponse.Should().NotBeNull();
        allItemsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        allItems.Should().NotBeNull();
        allItems!.Data.Should().BeEquivalentTo(seededBrandsDtos);
    }

    [Theory]
    [InlineData(1, 2)]
    public async Task Should_ReturnBrandsWithCustomPagination_When_GetAllEndpointIsCalledWithQueryParameters
        (int pageIndex, int pageSize)
    {
        // Act
        var response = await _client.GetAsync($"{Endpoint}/all?pageIndex={pageIndex}&pageSize={pageSize}");
        var result = await response.Content.ReadFromJsonAsync<PageResponse<BrandDto>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Data.Count().Should().Be(pageSize);
        result!.PaginationInfo.PageIndex.Should().Be(pageIndex);
        result.PaginationInfo.PageSize.Should().Be(pageSize);
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
    public async Task Should_ReturnBrand_When_GetBrandEndpointIsCalledWithValidId()
    {
        // Arrange
        var expectedBrand = EntitiesFixture.GetRandomEntity<Brand>();
        var brandId = expectedBrand.Id;

        // Act
        var response = await _client.GetAsync($"{Endpoint}/{brandId}");
        var result = await response.Content.ReadFromJsonAsync<BrandDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedBrand.ToDto());
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_GetEndpointIsCalledWithInvalidId()
    {
        // Arrange
        var invalidBrandId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"{Endpoint}/{invalidBrandId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    #endregion

    #region POST /{id} 

    [Fact]
    public async Task Should_CreateBrand_When_CreateBrandEndpointIsCalledWithValidData()
    {
        // Arrange
        var newBrand = DtosFixture.BrandUpdateDto;

        // Act
        var response = await _client.PostAsJsonAsync($"{Endpoint}", newBrand);
        var responseFromPostDto = await response.Content.ReadFromJsonAsync<BrandDto>();

        var responseFromGet = await _client.GetAsync($"{Endpoint}/{responseFromPostDto!.BrandId}");
        var responseFromGetDto = await responseFromGet.Content.ReadFromJsonAsync<BrandDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        responseFromGet.StatusCode.Should().Be(HttpStatusCode.OK);

        AssertDtosAreEqual(newBrand, responseFromPostDto);

        responseFromGetDto.Should().BeEquivalentTo(responseFromPostDto);
        responseFromPostDto.Should().NotBeNull();

        //Verify
        await _client.DeleteAsync($"{Endpoint}/{responseFromPostDto.BrandId}");
    }

    [Fact]
    public async Task Should_PublishBrandCreatedEvent_When_CreateBrandEndpointIsCalledWithValidData()
    {
        // Arrange
        var newBrand = DtosFixture.BrandUpdateDto;

        // Act
        var request = async () => await _client.PostAsJsonAsync($"{Endpoint}", newBrand);
        var (messagesPublished, response) = await _testHelper.GetPublishedMessagesCount<BrandCreatedEvent>(request);

        //Assert
        messagesPublished.Should().Be(1);

        //Verify
        var responseDto = await response.Content.ReadFromJsonAsync<BrandDto>();
        response.Should().NotBeNull();
        await _client.DeleteAsync($"{Endpoint}/{responseDto!.BrandId}");
    }

    [Fact]
    public async Task Should_Return400_When_CreatingBrandWithInvalidData()
    {
        // Arrange
        var invalidBrandData = DtosFixture.InvalidBrandUpdateDto;

        // Act
        var response = await _client.PostAsJsonAsync($"{Endpoint}/", invalidBrandData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    #endregion

    #region PUT
    
    [Fact]
    public async Task Should_UpdateBrand_When_UpdateBrandEndpointIsCalledWithValidData()
    {
        // Arrange
        var brandToUpdate = EntitiesFixture.GetRandomEntity<Brand>();
        var brandUpdateDto = DtosFixture.BrandUpdateDto;

        // Act
        var responseFromPut = await _client.PutAsJsonAsync($"{Endpoint}/{brandToUpdate.Id}", brandUpdateDto);
        var responseFromGet = await _client.GetAsync($"{Endpoint}/{brandToUpdate.Id}");
        var responseFromGetDto = await responseFromGet.Content.ReadFromJsonAsync<BrandDto>();

        // Assert
        responseFromPut.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseFromGet.StatusCode.Should().Be(HttpStatusCode.OK);

        responseFromGetDto.Should().NotBeNull();
        AssertDtosAreEqual(brandUpdateDto, responseFromGetDto!);

        //Verify
        await UpdateBrandFromEntity(brandToUpdate);
    }

    [Fact]
    public async Task Should_PublishBrandUpdatedEvent_When_UpdateBrandEndpointIsCalledWithValidData()
    {
        // Arrange
        var seededBrand = EntitiesFixture.GetRandomEntity<Brand>();
        var newBrand = DtosFixture.BrandUpdateDto;

        // Act
        var request = async () => await _client
            .PutAsJsonAsync($"{Endpoint}/{seededBrand.Id}", newBrand);
        var (messagesPublished, _) = await _testHelper.GetPublishedMessagesCount<BrandUpdatedEvent>(request);

        //Assert
        messagesPublished.Should().Be(1);

        //Verify
        await UpdateBrandFromEntity(seededBrand);
    }

    [Fact]
    public async Task Should_Return400_When_CallingUpdateWithInvalidData()
    {
        // Arrange
        var seededBrand = EntitiesFixture.GetRandomEntity<Brand>();
        var invalidBrandData = DtosFixture.InvalidBrandUpdateDto;

        // Act
        var response = await _client.PutAsJsonAsync($"{Endpoint}/{seededBrand.Id}", invalidBrandData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    #endregion

    #region DELETE /{id} 

    [Fact]
    public async Task Should_DeleteBrand_When_DeleteBrandEndpointIsCalledWithValidId()
    {
        // Arrange
        var createdBrandId = await PostTestBrand();

        // Act
        var response = await _client.DeleteAsync($"{Endpoint}/{createdBrandId}");
        var brandFromGetResponse = await _client.GetAsync($"{Endpoint}/{createdBrandId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        brandFromGetResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
    }

    [Fact]
    public async Task Should_PublishBrandDeletedEvent_When_BrandIsDeleted()
    {
        // Arrange
        var createdBrandId = await PostTestBrand();

        // Act
        var request = async () => await _client.DeleteAsync($"{Endpoint}/{createdBrandId}");
        var (messagesPublished, _) = await _testHelper.GetPublishedMessagesCount<BrandDeletedEvent>(request);

        // Assert
        messagesPublished.Should().Be(1);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_DeleteBrandEndpointIsCalledWithInvalidId()
    {
        // Arrange
        var invalidBrandId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"{Endpoint}/{invalidBrandId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    #endregion

    #region Helpers 

    private void AssertDtosAreEqual(BrandUpdateDto brandUpdateDto, BrandDto brandDto)
    {
        brandUpdateDto.Name.Should().BeEquivalentTo(brandDto.Name);
        brandUpdateDto.Description.Should().BeEquivalentTo(brandDto.Description);
        brandDto.Image.Should().BeEquivalentTo(brandDto.Image);
    }

    private async Task<Guid> PostTestBrand()
    {
        var brandToCreate = DtosFixture.BrandUpdateDto;

        var createBrandResponse = await _client.PostAsJsonAsync($"{Endpoint}/", brandToCreate);
        var createdBrand = await createBrandResponse.Content.ReadFromJsonAsync<BrandDto>();
        return createdBrand!.BrandId;
    }

    private async Task UpdateBrandFromEntity(Brand brand)
    {
        var brandUpdateDto = new BrandUpdateDto
        {
            Name = brand.Name.Value,
            Description = brand.Description.Value,
            Image = brand.Image.Value
        };

        await _client.PutAsJsonAsync($"{Endpoint}/{brand.Id}", brandUpdateDto);
    }
    
    #endregion
}