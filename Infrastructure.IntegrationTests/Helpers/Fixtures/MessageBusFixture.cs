using Infrastructure.IntegrationTests.Helpers.Extensions;
using Infrastructure.MessageBroker.EventBus;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.RabbitMq;

namespace Infrastructure.IntegrationTests.Helpers.Fixtures;

public class MessageBusFixture : IAsyncLifetime
{
    public IServiceProvider ServiceProvider { get; private set; }    
    
    private const string RabbitMqVersion = "3.11-management-alpine";
    private const string Username = "guest";
    private const string Password = "guest";
    private const string Hostname = "warehouse_mq";
    
    private readonly RabbitMqContainer _container = new RabbitMqBuilder()
        .WithImage($"rabbitmq:{RabbitMqVersion}")
        .WithHostname(Hostname)
        .WithUsername(Username)
        .WithPassword(Password)
        .Build();

    private IServiceCollection _services;
    
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        
        var settings = GetSettings();
        
        _services = new ServiceCollection();
        ServiceProvider = _services.AddTestMessaging(settings).BuildServiceProvider(true);
    }

    public IEventBus GetEventBus()
    {
        
        using var scope = ServiceProvider.CreateScope(); // Create a scope
        var scopedServiceProvider = scope.ServiceProvider; // Get the scoped service provider

        var publishEndpoint = scopedServiceProvider.GetRequiredService<IPublishEndpoint>(); // Use the scoped service provider to resolve the scoped service

        return new EventBus(publishEndpoint);
    }

    public ITestHarness GetTestHarness()
    {
        return ServiceProvider.GetRequiredService<ITestHarness>();
    }
    
    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }

    private MessageBrokerSettings GetSettings()
    {
        var settings = new MessageBrokerSettings
        {
            Host = new Uri($"amqp://127.0.0.1:{_container.GetMappedPublicPort(5672)}"),
            Username = Username,
            Password = Password
        };
        return settings;
    }

}