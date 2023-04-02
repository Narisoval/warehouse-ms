using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.IntegrationTests.Helpers.Extensions.DependencyInjection;

public static class DatabaseInjection
{
   public static IServiceCollection SetUpDbContext(this IServiceCollection services, string connectionString)
   {
       services.RemoveExistingDbContext();
       
       services.AddNewDbContext(connectionString);
        
       return services;
   }

   private static IServiceCollection RemoveExistingDbContext(this IServiceCollection services)
   {
        var dbContextDescriptor = services.SingleOrDefault(
            d => d.ServiceType ==
                typeof(DbContextOptions<WarehouseDbContext>));

        services.Remove(dbContextDescriptor!);
        
        return services;
   }

   private static IServiceCollection AddNewDbContext(this IServiceCollection services, string connectionString)
   {
        services.AddDbContext<WarehouseDbContext>((_, options) =>
        {
            options.UseNpgsql(connectionString);
        });
        
        return services;
   }
}