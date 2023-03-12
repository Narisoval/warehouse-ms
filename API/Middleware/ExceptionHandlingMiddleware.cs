using System.Text.Json;
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
        var problemDetails = ConstructServerErrorProblemDetails();

        if(problemDetails.Status != null)
            context.Response.StatusCode = (int)problemDetails.Status;
        
        await WriteProblem(problemDetails, context);
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