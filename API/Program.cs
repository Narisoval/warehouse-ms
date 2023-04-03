using Infrastructure.DependencyInjection;
using Infrastructure.Services;
using Warehouse.API.Helpers.Extensions;
using Warehouse.API.Helpers.Extensions.ApplicationBuilderExtensions;
using Warehouse.API.Helpers.Extensions.DependencyInjection;
using Warehouse.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLoggingToElasticSearch();

builder.Services.AddControllersWithBinders();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddLoggingEventBus();

builder.Services.Configure<ImageStoreConfiguration>(builder.Configuration.GetSection("ImageStore"));

builder.Services.AddS3Service();

builder.Services.AddApiVersions();

builder.Services.AddSwagger();

var app = builder.Build();

app.UseSwaggerIfDevelopment();

app.UseExceptionHandlingMiddleware();

app.UseRequestLogging();

app.MapControllers();

app.Run();

//For tests
public partial class Program { }