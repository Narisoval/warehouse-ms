using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.Helpers.Binders;

namespace Warehouse.API.Helpers;

public static class DependencyInjection
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
        });
        services.AddSwaggerExamplesFromAssemblyOf<Program>();
        
        return services;
    }

    public static IServiceCollection AddControllersWithBinders(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.ModelBinderProviders.Insert(0, new ModelBindersProvider());
        });
        
        return services;
    }
}