using Microsoft.AspNetCore.Mvc.Versioning;

namespace Warehouse.API.Helpers.Extensions.DependencyInjection;

public static class ApiVersioningInjection
{
    public static IServiceCollection AddApiVersions(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
        
        return services;    
    }
}