using Infrastructure;
using Serilog;
using Warehouse.API.Helpers.Extensions;
using Warehouse.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLoggingToElasticSearch();

builder.Services.AddControllersWithBinders();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddSwagger();

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