using API.IntegrationTests.Helpers.Extensions.DependencyInjection;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace API.IntegrationTests.Helpers.Fixtures;

public class WarehouseWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string RabbitMqVersion = "3.11-management-alpine";
    private const string Username = "guest";
    private const string Password = "guest";
    private const string Hostname = "warehouse_mq";
    
    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage($"rabbitmq:{RabbitMqVersion}")
        .WithHostname(Hostname)
        .WithUsername(Username)
        .WithPassword(Password)
        .Build();
    
    private const string PostgresVersion = "15.2-alpine";
    
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage($"postgres:{PostgresVersion}")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var messageBrokerSettings = GetMessageBrokerSettings();

            services.SetUpTestMessaging(messageBrokerSettings);


            services.SetUpDbContext(_dbContainer.GetConnectionString());

            services.SetUpMockStorageProvider();

        }).ConfigureLogging(logging => logging.ClearProviders());
    }
    
    private MessageBrokerSettings GetMessageBrokerSettings()
    {
        var settings = new MessageBrokerSettings
        {
            Host = new Uri($"amqp://127.0.0.1:{_rabbitMqContainer.GetMappedPublicPort(5672)}"),
            Username = Username,
            Password = Password
        };
        return settings;
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var contextOptions = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseNpgsql(_dbContainer.GetConnectionString())
            .Options;
            
        var context = new WarehouseDbContext(contextOptions);
        
        await context.Database.EnsureCreatedAsync();

        var dbSeeder = new DbDataSeeder(context);
        await dbSeeder.SeedTestData();
        await _rabbitMqContainer.StartAsync();
        
    }
    
    public new async Task DisposeAsync()
    {
        await _rabbitMqContainer.StopAsync();
        await _dbContainer.StopAsync();
    }
}