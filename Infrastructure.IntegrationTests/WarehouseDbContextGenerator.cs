using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests;

public class WarehouseDbContextGenerator : IAsyncLifetime
{
    private readonly TestcontainerDatabase _postgresqlContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "db",
            Username = "postgres",
            Password = "postgres",
        })
        .Build();

    public WarehouseDbContext GetContext()
    {
        var contextOptions = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseNpgsql(_postgresqlContainer.ConnectionString)
            .Options;
            
        var context = new WarehouseDbContext(contextOptions);
        
        context.Database.EnsureCreated();
        
        return context;
    }
    
    
    public async Task InitializeAsync()
    {
        await _postgresqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgresqlContainer.StopAsync();
    } 
}