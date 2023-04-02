using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace API.IntegrationTests.Helpers.Extensions.DependencyInjection;

public static class MessagingInjection
{
    public static IServiceCollection SetUpTestMessaging(this IServiceCollection services,
        MessageBrokerSettings settings)
    {
        services.RemoveMassTransit();

        services.AddTestMassTransit(settings);

        services.AddInMemoryTestHarness();

        return services;
    }
    
    private static IServiceCollection RemoveMassTransit(this IServiceCollection services)
    {
        var massTransitDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IBusControl));
        if (massTransitDescriptor != null)
        {
            services.Remove(massTransitDescriptor);
        }

        return services;
    }

    private static IServiceCollection AddTestMassTransit(this IServiceCollection services, MessageBrokerSettings settings)
    {
        services.AddMassTransitTestHarness(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.ConfigureEndpoints(context);

                configurator.Host(settings.Host, h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });
            });
        });
        return services;
    }

    private static IServiceCollection AddInMemoryTestHarness(this IServiceCollection services)
    {
        var harness = new InMemoryTestHarness();
        services.AddSingleton(harness);
        return services;
    }
}