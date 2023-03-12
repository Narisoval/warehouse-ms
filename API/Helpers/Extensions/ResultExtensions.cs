using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Warehouse.API.Helpers.Extensions;

public static class ResultExtensions
{
    public static void AddModelErrors(this List<IError> errors, ModelStateDictionary modelState, string key)
    {
        foreach (var error in errors)
        {
            modelState.AddModelError(key,error.Message);    
        }
    }
    
}