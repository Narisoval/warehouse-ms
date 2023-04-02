using System.Net;
using System.Net.Http.Json;
using API.IntegrationTests.Helpers;
using API.IntegrationTests.Helpers.Fixtures;
using Domain.Entities;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.API.DTO.CategoryDtos;
using Warehouse.API.DTO.PaginationDtos;
using Warehouse.API.Helpers.Mapping;
using Warehouse.API.Messaging.Events.CategoryEvents;

namespace API.IntegrationTests.ControllerTests;

public class CategoriesControllerTests : IClassFixture<WarehouseWebApplicationFactory>
{
    private readonly HttpClient _client;


    private readonly ControllerTestHelper _testHelper;

    private const string Endpoint = "api/v1/categories";
    
    public CategoriesControllerTests(WarehouseWebApplicationFactory factory)
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
    public async Task Should_ReturnAllCategories_When_GetAllEndpointIsCalled()
    {
        // Arrange
        var oneItemResponse = await _client.GetAsync($"{Endpoint}/all?pageIndex=1&pageSize=1");
        var oneItemPageResponse = await oneItemResponse.Content.ReadFromJsonAsync<PageResponse<CategoryDto>>();
        oneItemPageResponse.Should().NotBeNull();
        var pagesSize = oneItemPageResponse!.PaginationInfo.TotalRecords;
        
        //Act
        var allItemsResponse = await _client.GetAsync($"{Endpoint}/all?pageIndex={1}&pageSize={pagesSize}");
        var allItems = await allItemsResponse.Content.ReadFromJsonAsync<PageResponse<CategoryDto>>();
        var seededCategoriesDtos = EntitiesFixture.Categories.Select(category => category.ToDto());
        
        //Assert
        oneItemPageResponse.Should().NotBeNull();
        allItemsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        allItems.Should().NotBeNull();
        allItems!.Data.Should().BeEquivalentTo(seededCategoriesDtos);
    }
    
    [Theory]
    [InlineData(1, 2)]
    public async Task Should_ReturnCategoriesWithCustomPagination_When_GetAllEndpointIsCalledWithQueryParameters
        (int pageIndex, int pageSize)
    {
        // Act
        var response = await _client.GetAsync($"{Endpoint}/all?pageIndex={pageIndex}&pageSize={pageSize}");
        var result = await response.Content.ReadFromJsonAsync<PageResponse<CategoryDto>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Data.Count().Should().Be(pageSize);
        result.PaginationInfo.PageIndex.Should().Be(pageIndex);
        result.PaginationInfo.PageSize.Should().Be(pageSize);
    }
    
    [Theory]
    [InlineData(0, 6)]
    [InlineData(6, 0)]
    [InlineData(0, 0)]
    [InlineData(-1, 9)]
    [InlineData(9, -1)]
    [InlineData(-1, -1)]
    public async Task Should_ReturnBadRequest_When_GetAllEndpointIsCalledWithInvalidQueryParameters(int pageIndex, int pageSize)
    {
        // Act
        var response = await _client.GetAsync($"{Endpoint}/all?pageIndex={pageIndex}&pageSize={pageSize}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    #endregion

    #region GET /{id} 
    
    [Fact]
    public async Task Should_ReturnCategory_When_GetCategoryEndpointIsCalledWithValidId()
    {
        // Arrange
        var expectedCategory = EntitiesFixture.Categories.First();
        var categoryId = expectedCategory.Id;

        // Act
        var response = await _client.GetAsync($"{Endpoint}/{categoryId}");
        var result = await response.Content.ReadFromJsonAsync<CategoryDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedCategory.ToDto());
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
    public async Task Should_CreateCategory_When_CreateCategoryEndpointIsCalledWithValidData()
    {
        // Arrange
        var newCategory = DtosFixture.CategoryUpdateDto;

        // Act
        var responseFromPost = await _client.PostAsJsonAsync($"{Endpoint}", newCategory);
        var responseFromPostDto = await responseFromPost.Content.ReadFromJsonAsync<CategoryDto>();
        
        var responseFromGet = await _client.GetAsync($"{Endpoint}/{responseFromPostDto!.CategoryId}");
        var responseFromGetDto  = await responseFromGet.Content.ReadFromJsonAsync<CategoryDto>();
        
        // Assert
        responseFromPost.StatusCode.Should().Be(HttpStatusCode.Created);
        responseFromGet.StatusCode.Should().Be(HttpStatusCode.OK);
        
        AssertDtosAreEqual(newCategory,responseFromPostDto); 
        
        responseFromGetDto.Should().NotBeNull();
        responseFromGetDto.Should().BeEquivalentTo(responseFromPostDto);
        
        //Verify
        var deletionResponse = await _client.DeleteAsync($"{Endpoint}/{responseFromPostDto.CategoryId}");
        deletionResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Should_PublishCategoryCreatedEvent_When_CreateCategoryEndpointIsCalledWithValidData()
    {
        // Arrange
        var newCategory = DtosFixture.CategoryUpdateDto;

        // Act
        var request = async () => await _client.PostAsJsonAsync($"{Endpoint}", newCategory);
        var (messagesPublished,response) = await _testHelper.GetPublishedMessagesCount<CategoryCreatedEvent>(request);
        
        //Assert
        messagesPublished.Should().Be(1);
        
        //Verify
        var responseDto = await response.Content.ReadFromJsonAsync<CategoryDto>();
        response.Should().NotBeNull();
        var deletionResponse = await _client.DeleteAsync($"{Endpoint}/{responseDto!.CategoryId}");
        deletionResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_Return400_When_CreatingCategoryWithInvalidData()
    {
        // Arrange
        var invalidCategoryData = DtosFixture.InvalidCategoryUpdateDto; 

        // Act
        var response = await _client.PostAsJsonAsync($"{Endpoint}/", invalidCategoryData);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);       
    }
    
    #endregion

    #region PUT

    [Fact]
    public async Task Should_UpdateCategory_When_UpdateCategoryEndpointIsCalledWithValidData()
    {
        // Arrange
        var categoryToUpdate = EntitiesFixture.GetRandomEntity<Category>();
        var categoryUpdateDto = DtosFixture.CategoryUpdateDto;

        // Act
        var responseFromPut = await _client.PutAsJsonAsync($"{Endpoint}/{categoryToUpdate.Id}", categoryUpdateDto);
        var responseFromGet = await _client.GetAsync($"{Endpoint}/{categoryToUpdate.Id}");
        var responseFromGetDto = await responseFromGet.Content.ReadFromJsonAsync<CategoryDto>();
        
        // Assert
        responseFromPut.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseFromGet.StatusCode.Should().Be(HttpStatusCode.OK);
        
        responseFromGetDto.Should().NotBeNull();
        AssertDtosAreEqual(categoryUpdateDto,responseFromGetDto!);
        
        //Verify
        await UpdateCategoryFromEntity(categoryToUpdate);
    }

    [Fact]
    public async Task Should_PublishCategoryUpdatedEvent_When_UpdateCategoryEndpointIsCalledWithValidData()
    {
        // Arrange
        var seededCategory = EntitiesFixture.GetRandomEntity<Category>();
        var newCategory = DtosFixture.CategoryUpdateDto;
        
        // Act
        var request = async () => await _client
            .PutAsJsonAsync($"{Endpoint}/{seededCategory.Id}", newCategory);
        
        var (messagesPublished, _) = await _testHelper.GetPublishedMessagesCount<CategoryUpdatedEvent>(request);
            
        //Assert
        messagesPublished.Should().Be(1);
        
        //Verify
        await UpdateCategoryFromEntity(seededCategory);
    }

    [Fact]
    public async Task Should_Return400_When_CallingUpdateWithInvalidData()
    {
        // Arrange
        var seededCategory = EntitiesFixture.GetRandomEntity<Category>();
        var invalidCategoryData = DtosFixture.InvalidCategoryUpdateDto; 

        // Act
        var response = await _client.PutAsJsonAsync($"{Endpoint}/{seededCategory.Id}", invalidCategoryData);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    #endregion

    #region DELETE /{id}

    [Fact]
    public async Task Should_DeleteCategory_When_DeleteCategoryEndpointIsCalledWithValidId()
    {
        // Arrange
        var createdCategoryId = await PostTestCategory();
        
        // Act
        var response = await _client.DeleteAsync($"{Endpoint}/{createdCategoryId}");
        var categoryFromGetResponse = await _client.GetAsync($"{Endpoint}/{createdCategoryId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        categoryFromGetResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
    }
    
    [Fact]
    public async Task Should_PublishCategoryDeletedEvent_When_CategoryIsDeleted()
    {
        // Arrange
        var createdCategoryId = await PostTestCategory();
        
        // Act
        var request = async () => await _client.DeleteAsync($"{Endpoint}/{createdCategoryId}");

        var (messagesPublished, _) = await _testHelper.GetPublishedMessagesCount<CategoryDeletedEvent>(request);
        
        // Assert
        messagesPublished.Should().Be(1);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_DeleteCategoryEndpointIsCalledWithInvalidId()
    {
        // Arrange
        var invalidCategoryId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"{Endpoint}/{invalidCategoryId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    #endregion

    #region Helpers
    
    private void AssertDtosAreEqual(CategoryUpdateDto categoryUpdateDto, CategoryDto categoryDto)
    {
        categoryUpdateDto.Name.Should().BeEquivalentTo(categoryDto.Name);
    }
    
    private async Task<Guid> PostTestCategory()
    {
        var categoryToCreate = DtosFixture.CategoryUpdateDto;
        
        var createCategoryResponse = await _client.PostAsJsonAsync($"{Endpoint}/", categoryToCreate);
        var createdCategory = await createCategoryResponse.Content.ReadFromJsonAsync<CategoryDto>();
        return createdCategory!.CategoryId;
    }
    
    private async Task UpdateCategoryFromEntity(Category category)
    {
        var categoryUpdateDto = new CategoryUpdateDto
        {
            Name = category.Name.Value
        };
        
        await _client.PutAsJsonAsync($"{Endpoint}/{category.Id}", categoryUpdateDto);
    }

    #endregion
}

