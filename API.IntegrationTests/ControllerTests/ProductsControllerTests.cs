using System.Net;
using System.Net.Http.Json;
using API.IntegrationTests.Helpers;
using API.IntegrationTests.Helpers.Fixtures;
using Domain.Entities;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.API.DTO.PaginationDtos;
using Warehouse.API.DTO.ProductDtos;
using Warehouse.API.Helpers.Mapping;
using Warehouse.API.Messaging.Events.ProductEvents;

namespace API.IntegrationTests.ControllerTests;

public class ProductsControllerTests : IClassFixture<WarehouseWebApplicationFactory>
{
    private readonly HttpClient _client;

    private readonly ControllerTestHelper _testHelper;

    private const string Endpoint = "api/products";

    public ProductsControllerTests(WarehouseWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var testHarness = factory.Services.GetRequiredService<ITestHarness>();
        var semaphore = new SemaphoreSlim(1, 1);

        _testHelper = new ControllerTestHelper(semaphore, testHarness);
    }

    #region GET /all 

    [Fact]
    public async Task Should_ReturnAllProducts_When_GetAllEndpointIsCalled()
    {
        // Arrange
        var oneItemResponse = await _client.GetAsync($"{Endpoint}/all?pageIndex=1&pageSize=1");
        var oneItemPageResponse = await oneItemResponse.Content.ReadFromJsonAsync<PageResponse<ProductDto>>();
        oneItemPageResponse.Should().NotBeNull();
        var pagesSize = oneItemPageResponse!.PaginationInfo.TotalRecords;

        //Act
        var allItemsResponse = await _client.GetAsync($"{Endpoint}/all?pageIndex={1}&pageSize={pagesSize}");
        var allItems = await allItemsResponse.Content.ReadFromJsonAsync<PageResponse<ProductDto>>();
        var seededProductsDtos = EntitiesFixture.Products.Select(product => product.ToDto());

        //Assert
        oneItemPageResponse.Should().NotBeNull();
        allItemsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        allItems.Should().NotBeNull();
        allItems!.Data.Should().BeEquivalentTo(seededProductsDtos);
    }

    [Fact]
    public async Task Should_ReturnProductWithAllRelatedEntities_When_GettingAllProducts()
    {
        // Arrange
        const int pageIndex = 1;
        const int pagSize = 1;

        //Act
        var oneItemResponse = await _client.GetAsync($"{Endpoint}/all?pageIndex={pageIndex}&pageSize={pagSize}");
        var oneItemPageResponse = await oneItemResponse.Content.ReadFromJsonAsync<PageResponse<ProductDto>>();

        //Assert
        oneItemPageResponse.Should().NotBeNull();
        var dto = oneItemPageResponse!.Data.First();
        dto.Brand.Should().NotBeNull();
        dto.Provider.Should().NotBeNull();
        dto.Category.Should().NotBeNull();
    }

    [Theory]
    [InlineData(1, 5)]
    public async Task Should_ReturnProductsWithCustomPagination_When_GetAllEndpointIsCalledWithQueryParameters
        (int pageIndex, int pageSize)
    {
        // Act
        var response = await _client.GetAsync($"{Endpoint}/all?pageIndex={pageIndex}&pageSize={pageSize}");
        var result = await response.Content.ReadFromJsonAsync<PageResponse<ProductDto>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
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
    public async Task Should_ReturnProduct_When_GetProductEndpointIsCalledWithValidId()
    {
        // Arrange
        var expectedProduct = EntitiesFixture.GetRandomEntity<Product>();
        var productId = expectedProduct.Id;

        // Act
        var response = await _client.GetAsync($"{Endpoint}/{productId}");
        var result = await response.Content.ReadFromJsonAsync<ProductDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedProduct.ToDto());
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_GetEndpointIsCalledWithInvalidId()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"{Endpoint}/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnProductWithAllRelatedEntities_When_GettingProductById()
    {
        // Arrange
        var expectedProduct = EntitiesFixture.GetRandomEntity<Product>();
        var productId = expectedProduct.Id;

        //Act
        var response = await _client.GetAsync($"{Endpoint}/{productId}");
        var responseDto = await response.Content.ReadFromJsonAsync<ProductDto>();

        //Assert
        responseDto.Should().NotBeNull();
        responseDto!.Brand.Should().NotBeNull();
        responseDto.Provider.Should().NotBeNull();
        responseDto.Category.Should().NotBeNull();
    }
    
    #endregion 
    
    #region POST /{id} 

    [Fact]
    public async Task Should_CreateProduct_When_PostEndpointIsCalledWithValidData()
    {
        // Arrange
        var newProduct = DtosFixture.ProductUpdateDto;

        // Act
        var responseFromPost = await _client.PostAsJsonAsync($"{Endpoint}/", newProduct);
        var responseFromPostDto = await responseFromPost.Content.ReadFromJsonAsync<ProductDto>();

        var responseFromGet = await _client.GetAsync($"{Endpoint}/{responseFromPostDto!.ProductId}");
        var responseFromGetDto  = await responseFromGet.Content.ReadFromJsonAsync<ProductDto>();
        
        // Assert
        responseFromPost.StatusCode.Should().Be(HttpStatusCode.Created);
        responseFromPostDto.Should().NotBeNull();

        AssertDtosAreEqual(newProduct, responseFromPostDto);

        responseFromPostDto.Should().BeEquivalentTo(responseFromGetDto);
        responseFromGetDto.Should().NotBeNull();

        //Verify
        var deletionResponse = await _client.DeleteAsync($"{Endpoint}/{responseFromPostDto.ProductId}");
        deletionResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_PublishProductCreatedEvent_When_CreateProductEndpointIsCalledWithValidData()
    {
        // Arrange
        var newProduct = DtosFixture.ProductUpdateDto;

        // Act
        var request = async () => await _client.PostAsJsonAsync($"{Endpoint}/", newProduct);
        var (messagesPublished,response) = await _testHelper.GetPublishedMessagesCount<ProductCreatedEvent>(request);
        
        //Assert
        messagesPublished.Should().Be(1);
        
        //Verify
        var responseDto = await response.Content.ReadFromJsonAsync<ProductDto>();
        response.Should().NotBeNull();
        var deletionResponse = await _client.DeleteAsync($"{Endpoint}/{responseDto!.ProductId}");
        deletionResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_When_PostEndpointIsCalledWithInvalidData()
    {
        // Arrange
        var invalidProduct = DtosFixture.InvalidProductUpdateDto;

        // Act
        var response = await _client.PostAsJsonAsync(Endpoint, invalidProduct);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Should_ReturnBadRequest_When_PostEndpointIsCalledWithWrongForeignKeys()
    {
        // Arrange
        var invalidProduct = DtosFixture.ProductUpdateWithWrongForeignKeys;

        // Act
        var response = await _client.PostAsJsonAsync(Endpoint, invalidProduct);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    #endregion
    
    [Fact]
    public async Task Should_UpdateProduct_When_PutEndpointIsCalledWithValidData()
    {
        // Arrange
        var existingProduct = EntitiesFixture.GetRandomEntity<Product>();
        var updatedProduct = DtosFixture.ProductUpdateDto;

        // Act
        var responseFromPut = await _client.PutAsJsonAsync($"{Endpoint}/{existingProduct.Id}", updatedProduct);
        
        var responseFromGet = await _client.GetAsync($"{Endpoint}/{existingProduct.Id}");
        var responseFromGetDto = await responseFromGet.Content.ReadFromJsonAsync<ProductDto>();

        // Assert
        responseFromPut.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseFromGet.StatusCode.Should().Be(HttpStatusCode.OK);

        responseFromGetDto.Should().NotBeNull();
        AssertDtosAreEqual(updatedProduct,responseFromGetDto!);
        
        //Verify
        await UpdateProductFromEntity(existingProduct);
    }
    
    [Fact]
    public async Task Should_PublishProductUpdatedEvent_When_UpdateProductCalledWithValidData()
    {
        // Arrange
        var seededProduct = EntitiesFixture.GetRandomEntity<Product>();
        var productWithNewValues = DtosFixture.ProductUpdateDto;
        
        // Act
        var request = async () => await _client
            .PutAsJsonAsync($"{Endpoint}/{seededProduct.Id}", productWithNewValues);
        
        var (messagesPublished, _) = await _testHelper.GetPublishedMessagesCount<ProductUpdatedEvent>(request);
            
        //Assert
        messagesPublished.Should().Be(1);
        
        //Verify
        await UpdateProductFromEntity(seededProduct);
    }

    [Fact]
    public async Task Should_Return400_When_CallingUpdateWithInvalidData()
    {
        // Arrange
        var seededProduct = EntitiesFixture.GetRandomEntity<Product>();
        var invalidProductData = DtosFixture.InvalidProductUpdateDto; 

        // Act
        var response = await _client.PutAsJsonAsync($"{Endpoint}/{seededProduct.Id}", invalidProductData);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_PutEndpointIsCalledWithInvalidId()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var productUpdateDto = DtosFixture.ProductUpdateDto;

        // Act
        var response = await _client.PutAsJsonAsync($"{Endpoint}/{invalidId}", productUpdateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_DeleteProduct_When_DeleteEndpointIsCalledWithValidId()
    {
        // Arrange
        var createdProductId = await PostTestProduct();

        // Act
        var response = await _client.DeleteAsync($"{Endpoint}/{createdProductId}");
        var getResponse = await _client.GetAsync($"{Endpoint}/{createdProductId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_DeleteEndpointIsCalledWithInvalidId()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"{Endpoint}/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Should_PublishCategoryDeletedEvent_When_CategoryIsDeleted()
    {
        // Arrange
        var createdProductId = await PostTestProduct();
        
        // Act
        var request = async () => await _client.DeleteAsync($"{Endpoint}/{createdProductId}");

        var (messagesPublished, _) = await _testHelper.GetPublishedMessagesCount<ProductDeletedEvent>(request);
        
        // Assert
        messagesPublished.Should().Be(1);
    }
    
    private void AssertDtosAreEqual(ProductUpdateDto updateDto, ProductDto dto)
    {
        updateDto.Name.Should().BeEquivalentTo(dto.Name);
        updateDto.Quantity.Should().Be(dto.Quantity);
        updateDto.Description.Should().BeEquivalentTo(dto.Description);
        updateDto.FullPrice.Should().Be(dto.FullPrice);
        updateDto.MainImage.Should().BeEquivalentTo(dto.MainImage);
        updateDto.Images.Should().BeEquivalentTo(dto.Images);
        updateDto.Sale.Should().Be(dto.Sale);
        updateDto.IsActive.Should().Be(dto.IsActive);
    }
    
    private async Task UpdateProductFromEntity(Product existingProduct)
    {
        var productImages = 
            existingProduct.Images!.Select(image => image.Image.Value).ToList();

        var productUpdateDto = new ProductUpdateDto
        {
            Name = existingProduct.Name.Value,
            Quantity = existingProduct.Quantity.Value,
            FullPrice = existingProduct.FullPrice.Value,
            Description = existingProduct.Description.Value,
            MainImage = existingProduct.MainImage.Value,
            Images = productImages,
            Sale = existingProduct.Sale.Value,
            IsActive = existingProduct.IsActive,
            CategoryId = existingProduct.CategoryId,
            ProviderId = existingProduct.ProviderId,
            BrandId = existingProduct.BrandId
        };
        
        var responseFromPut = await _client
            .PutAsJsonAsync($"{Endpoint}/{existingProduct.Id}", productUpdateDto);
        responseFromPut.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    private async Task<Guid> PostTestProduct()
    {
        var productToCreate = DtosFixture.ProductUpdateDto;

        var postProductResponse = await _client.PostAsJsonAsync(Endpoint, productToCreate);
        var createdProductDto = await postProductResponse.Content.ReadFromJsonAsync<ProductDto>();
        
        
        return createdProductDto!.ProductId;
    }
}

