using Infrastructure.DependencyInjection;
using Infrastructure.MessageBroker.EventBus;
using Warehouse.API.Helpers.Extensions;
using Warehouse.API.Middleware;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLoggingToElasticSearch();

builder.Services.AddControllersWithBinders();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddSwagger();

builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddTransient<IEventBus, EventBus>();

builder.Services.AddLoggingEventBus();

builder.Services.Configure<ImageStoreConfiguration>(builder.Configuration.GetSection("ImageStore"));

builder.Services.AddS3Service();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandlingMiddleware();
app.UseRequestLogging();
app.MapControllers();
app.Run();

//For tests
public partial class Program { }