using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Infrastructure.IntegrationTests.Helpers.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private const string PostgresVersion = "15.2-alpine";

    private const int PublicPort = 5432;
    public IUnitOfWork UnitOfWork { get; private set; }

    public DbDataSeeder DbDataSeeder { get; private set; }
    
    public WarehouseDbContext Context;
    
    private readonly PostgreSqlContainer _postgresqlContainer = new PostgreSqlBuilder()
        .WithImage($"postgres:{PostgresVersion}")
        .Build();


    public async Task InitializeAsync()
    {
        await _postgresqlContainer.StartAsync();
        
        Console.WriteLine("CONTAINER CREATED ON PORT " + _postgresqlContainer.GetMappedPublicPort(PublicPort));
        
        var contextOptions = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseNpgsql(_postgresqlContainer.GetConnectionString())
            .Options;
            
        Context = new WarehouseDbContext(contextOptions);
        
        await Context.Database.EnsureCreatedAsync();
        
        await SeedTestData();
        
        UnitOfWork = new UnitOfWork(Context);
    }

    private async Task SeedTestData()
    {
        DbDataSeeder = new DbDataSeeder(Context);
        await DbDataSeeder.SeedTestData();
    }
    
    public async Task DisposeAsync()
    {
        await _postgresqlContainer.StopAsync();
    } 
    
}