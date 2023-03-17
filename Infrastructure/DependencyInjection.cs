using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddWarehouseDbContext(configuration);
         
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static IServiceCollection AddWarehouseDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("postgres");
        services.AddDbContext<WarehouseDbContext>(
            optionsAction =>
            {
                optionsAction.UseNpgsql(connectionString);
            });
        
        MigrateDb(services);

        return services;
    }

    private static void MigrateDb(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetService<WarehouseDbContext>();
        dbContext?.Database.Migrate();
    }
}