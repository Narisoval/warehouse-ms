using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Warehouse.API.Helpers.Extensions;

public static class HostBuilderExtensions
{
    
    public static IHostBuilder UseLoggingToElasticSearch(this IHostBuilder builder)
    {
        builder.UseSerilog((context, configuration) =>
        {
                configuration.WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                    new Uri(context.Configuration["ElasticConfiguration:Uri"]))
                {
                    IndexFormat =
                        $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 2
                })
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(context.Configuration);
        });
        
        return builder;
    }

}