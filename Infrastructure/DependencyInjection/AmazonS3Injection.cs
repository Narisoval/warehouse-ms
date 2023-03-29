using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class AmazonS3Injection
{
    public static IServiceCollection AddS3Service(this IServiceCollection services)
    {
        services.AddAWSService<IAmazonS3>(new AWSOptions
        { 
            Region = RegionEndpoint.USWest2,
        });

        services.AddTransient<IStorageService, S3StorageService>();
        return services;
    }
}