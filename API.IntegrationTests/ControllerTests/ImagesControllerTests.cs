using System.Net;
using System.Net.Http.Json;
using System.Text;
using API.IntegrationTests.Helpers.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using API.IntegrationTests.Helpers.Mocks;
using Moq;
using Warehouse.API.DTO;

namespace API.IntegrationTests.ControllerTests;

public class ImagesControllerTests : IClassFixture<WarehouseWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ImagesControllerTests(WarehouseWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_ReturnsImageFileDtos_When_EndpointCalledWithOneFile()
    {
        // Arrange
        var testFile = GetTestsFile();
        var formContent = CreateMultipartFormDataContent(new List<IFormFile> { testFile.Object });

        //Act
        var response = await _client.PostAsync("/api/images", formContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var imageFileDtos = await response.Content.ReadFromJsonAsync<IEnumerable<ImageFileDto>>();
        var imageFileDtosList = imageFileDtos?.ToList();

        imageFileDtosList!.Should().NotBeNull();
        imageFileDtosList!.Should().NotBeEmpty();
        imageFileDtosList!.Count.Should().Be(1);
    }

    [Theory]
    [InlineData("image/jpeg", true)]
    [InlineData("image/png", true)]
    [InlineData("image/webp", true)]
    [InlineData("image/svg+xml", true)]
    [InlineData("application/pdf", false)]
    [InlineData("image/gif", false)]
    [InlineData("text/plain", false)]
    public async Task Should_ReturnAppropriateCode_When_UploadingFilesWithDifferentFileTypes
        (string contentType, bool isAllowed)
    {
        // Arrange
        var testFile = GetTestsFile(contentType: contentType);

        var formFiles = new List<IFormFile>() { testFile.Object };
        var formContent = CreateMultipartFormDataContent(formFiles);

        // Act
        var response = await _client.PostAsync("/api/images", formContent);

        // Assert
        response.StatusCode.Should().Be(isAllowed ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(2*1024*1024,true)] // 2 mb
    [InlineData(2*1024*1024+1,false)] // larger than 2 mb
    public async Task Should_ReturnAppropriateResponse_When_UploadingDifferentSizedFiles(long fileSize, bool isAllowed)
    {
        // Arrange
        var testFile = GetTestsFile();
        var fileStream = new MemoryStreamMock(fileSize);

        testFile.Setup(f => f.OpenReadStream()).Returns(fileStream);
        testFile.Setup(f => f.Length).Returns(fileSize);
        
        var formContent = CreateMultipartFormDataContent(new List<IFormFile>(){testFile.Object});

        // Act
        var response = await _client.PostAsync("/api/images", formContent);

        // Assert
        response.StatusCode.Should().Be(isAllowed ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData(10,true)]
    [InlineData(11,false)]
    public async Task Should_ReturnAppropriateResponse_When_UploadingDifferentNumberOfFiles(int filesNumber, bool isAllowed)
    {
        // Arrange
        var formFiles = new List<IFormFile>();

        for (int i = 0; i < filesNumber; i++)
        {
            var testFile = GetTestsFile();
            formFiles.Add(testFile.Object);
        }

        var formContent = CreateMultipartFormDataContent(formFiles);

        // Act
        var response = await _client.PostAsync("/api/images", formContent);

        // Assert
        response.StatusCode.Should().Be(isAllowed ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
    }


    private static MultipartFormDataContent CreateMultipartFormDataContent(List<IFormFile> formFiles)
    {
        var formContent = new MultipartFormDataContent();

        foreach (var file in formFiles)
        {
            var fileStream = file.OpenReadStream();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            formContent.Add(streamContent, "files", file.FileName);
        }

        return formContent;
    }
    
    private Mock<IFormFile> GetTestsFile(
    string content = "File content", string fileName = "test.jpg", string contentType = "image/jpeg")
    {
        var testFile = new Mock<IFormFile>();
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        
        testFile.Setup(f => f.FileName).Returns(fileName);
        testFile.Setup(f => f.ContentType).Returns(contentType);
        testFile.Setup(f => f.OpenReadStream()).Returns(fileStream);
        
        return testFile;
    }
}