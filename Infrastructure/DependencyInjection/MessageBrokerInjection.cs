using Infrastructure.MessageBroker;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.DependencyInjection;
public static class MessageBrokerInjection
{
    private const string sectionName = "MessageBroker";
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration)
    {
        var host = new Uri(configuration[$"{sectionName}:Host"]);
        var username = configuration[$"{sectionName}:Username"];
        var password = configuration[$"{sectionName}:Password"];
        
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.ConfigureEndpoints(context);

                configurator.Host(host, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
            });
        });

        return services;
    }
}