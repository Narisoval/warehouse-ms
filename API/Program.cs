using Infrastructure;
using Warehouse.API.Helpers;
using Warehouse.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

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

app.MapControllers();
app.Run();