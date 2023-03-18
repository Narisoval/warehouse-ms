using Infrastructure;
using Infrastructure.DependencyInjection;
using Infrastructure.EventBus;
using Infrastructure.MessageBroker;
using MassTransit;
using Microsoft.Extensions.Options;
using Warehouse.API.Helpers.Extensions;
using Warehouse.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLoggingToElasticSearch();

builder.Services.AddControllersWithBinders();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddSwagger();

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddMessageBroker();

builder.Services.AddTransient<IEventBus, EventBus>();

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