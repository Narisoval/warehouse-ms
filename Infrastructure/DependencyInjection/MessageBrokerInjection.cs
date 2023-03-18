using Infrastructure.MessageBroker;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.DependencyInjection;

public static class MessageBrokerInjection
{
   public static IServiceCollection AddMessageBroker(this IServiceCollection services)
   {
      
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.ConfigureEndpoints(context);
                
                MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();
                configurator.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });
            });
        });
        
        return services;
   }
}