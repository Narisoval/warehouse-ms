using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.OpenApi;

namespace Warehouse.API.Helpers.Extensions.DependencyInjection;

public static class SwaggerInjection
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
        });
        services.AddSwaggerExamplesFromAssemblyOf<Program>();
        
        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        
        
        return services;
    }
}