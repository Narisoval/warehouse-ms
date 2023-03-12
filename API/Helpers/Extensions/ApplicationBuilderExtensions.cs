using Serilog;

namespace Warehouse.API.Helpers.Extensions;

public static class ApplicationBuilderExtensions
{
   public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
   {
        builder.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticsContext, httpContext) =>
            {
                diagnosticsContext.Set("Authorization", httpContext.Request.Headers.Authorization, true);
            };
            
        });
        return builder;
   }
}