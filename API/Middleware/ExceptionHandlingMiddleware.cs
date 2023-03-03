using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.API.Middleware;

public static class ExceptionHandlingMiddleware
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp => 
        {
            errorApp.Run(HandleException);
        });
        return app;
    }

    private static async Task HandleException(HttpContext context)
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var problem = exception is ArgumentException or FormatException
            ? ConstructValidationProblemDetails(exception) : ConstructServerErrorProblemDetails();

        if(problem.Status != null)
            context.Response.StatusCode = (int)problem.Status;
        
        await WriteProblem(problem, context);
    }

    private static ProblemDetails ConstructValidationProblemDetails(Exception exception)
    {
        ProblemDetails problem = new()
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "Validation error",
            Title = "Validation Error has occured",
            Detail = exception.Message
        };
        return problem;
    }
    
    private static ProblemDetails ConstructServerErrorProblemDetails()
    {
        ProblemDetails problem = new()
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "Server error",
            Title = "Server error",
            Detail = "Internal server error has occurred" 
        };
        return problem;
    }

    private static async Task WriteProblem(ProblemDetails problem, HttpContext context)
    {
        var json = JsonSerializer.Serialize(problem);
        context.Response.ContentType = "application/json";
        
        await context.Response.WriteAsync(json);
    }
}