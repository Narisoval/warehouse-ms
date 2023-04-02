using API.IntegrationTests.Helpers.Mocks;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace API.IntegrationTests.Helpers.Extensions.DependencyInjection;

public static class StorageServiceInjection
{
    public static IServiceCollection SetUpMockStorageProvider(this IServiceCollection services)
    {
        services.RemoveAll<IStorageService>();
        services.AddSingleton<IStorageService, MockStorageService>();
        
        return services;
    }
}