using Infrastructure.DependencyInjection;
using Infrastructure.MessageBroker.EventBus;
using MassTransit;
using Warehouse.API.Helpers.Extensions;
using Warehouse.API.Messaging;
using Warehouse.API.Middleware;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLoggingToElasticSearch();

builder.Services.AddControllersWithBinders();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddSwagger();

builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddTransient<IEventBus, EventBus>();

builder.Services.AddTransient<IEventBus>(sp =>
{
    var publishEndpoint = sp.GetRequiredService<IPublishEndpoint>();
    var eventBus = new EventBus(publishEndpoint);
    var logger = sp.GetRequiredService<ILogger>();
    return new LoggingEventBus(eventBus, logger);
});
    
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