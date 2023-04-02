using Infrastructure.MessageBroker.EventBus;
using MassTransit;
using Swashbuckle.AspNetCore.Filters;
using Warehouse.API.Helpers.Binders;
using Warehouse.API.Messaging;
using ILogger = Serilog.ILogger; 

namespace Warehouse.API.Helpers.Extensions;

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
    
    public static IServiceCollection AddLoggingEventBus(this IServiceCollection services)
    {
        services.AddTransient<IEventBus, EventBus>();
        services.AddTransient<IEventBus>(sp =>
        {
            var publishEndpoint = sp.GetRequiredService<IPublishEndpoint>();
            var eventBus = new EventBus(publishEndpoint);
            var logger = sp.GetRequiredService<ILogger>();
            return new LoggingEventBus(eventBus, logger);
        });
        
        return services;
    }
    
}