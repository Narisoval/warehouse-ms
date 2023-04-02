using Serilog;

namespace Warehouse.API.Helpers.Extensions.ApplicationBuilderExtensions;

public static class ApplicationLoggingExtensions
{
   public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
   {
        builder.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticsContext, httpContext) =>
            {
                diagnosticsContext.Set("Authorization", httpContext.Request.Headers.Authorization, 
                    true);
            };
            
        });
        return builder;
   }
}