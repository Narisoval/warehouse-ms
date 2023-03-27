using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IntegrationTests.Helpers.Extensions;

public static class DependencyInjection
{
   public static IServiceCollection AddTestMessaging(this IServiceCollection services, MessageBrokerSettings settings)
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
}