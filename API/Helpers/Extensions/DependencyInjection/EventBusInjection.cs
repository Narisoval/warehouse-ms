using Infrastructure.MessageBroker.EventBus;
using MassTransit;
using Warehouse.API.Messaging;

using ILogger = Serilog.ILogger; 

namespace Warehouse.API.Helpers.Extensions.DependencyInjection;

public static class EventBusInjection
{
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