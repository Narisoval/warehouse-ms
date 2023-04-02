using Infrastructure.Services;

namespace API.IntegrationTests.Helpers.Mocks;

public class MockStorageService : IStorageService
{
   public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        await Task.Delay(10); // Simulate a small delay for the asynchronous operation
        return $"https://mockurl.com/{fileName}";
    }

    public async Task DeleteFileAsync(string fileKey)
    {
        await Task.Delay(10); // Simulate a small delay for the asynchronous operation
    }
}