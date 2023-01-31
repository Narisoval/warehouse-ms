using Infrastructure.Data; using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("postgres");
        services.AddDbContext<WarehouseDbContext>(
            optionsAction =>
            {
                optionsAction.UseNpgsql(connectionString);
            });
        
        return services;
    }
    
}