using Warehouse.API.Helpers.Binders;

namespace Warehouse.API.Helpers.Extensions.DependencyInjection;

public static class ControllersInjection
{
    public static IServiceCollection AddControllersWithBinders(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.ModelBinderProviders.Insert(0, new ModelBindersProvider());
        });
        
        return services;
    }
}