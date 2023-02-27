using System.Text.Json;
using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO.ProviderDtos;

namespace Warehouse.API.Common.Mapping.Bindings;

public class ProviderEntityModelBinder : BaseModelBinder
{
    public override async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (!CheckIfContentTypeIsJson(bindingContext))
            return;

        try
        {
            await BindProviderEntityAsync(bindingContext);
        }
        catch (JsonException ex)
        {
            bindingContext.ModelState.AddModelError(
                "ObjectFormatError",
                $"{ex.InnerException?.Message} The following json element caused a problem: {ex.Path}");
        }
    }

    private async Task BindProviderEntityAsync(ModelBindingContext bindingContext)
    {
        // Check if the route has a guid "id" route parameter
        if (TryGetIdFromRoute(bindingContext, out var guidId))
        {
            if (guidId != null) await BindFromDtoAsync(bindingContext, guidId.Value);
            return;
        }
        
        await BindFromUpdateDtoAsync(bindingContext);
    }

    private async Task BindFromDtoAsync(ModelBindingContext bindingContext, Guid id)
    {
        ProviderDto providerDto = (await bindingContext.HttpContext.Request
            .ReadFromJsonAsync<ProviderDto>())!;

        providerDto.ProviderId = id;

        ConvertDtoToEntity(providerDto, bindingContext);
    }
    
    private async Task BindFromUpdateDtoAsync(ModelBindingContext bindingContext)
    {
        ProviderUpdateDto providerUpdateDto = (await bindingContext.HttpContext.Request
            .ReadFromJsonAsync<ProviderUpdateDto>())!;

        ConvertUpdateDtoToEntity(providerUpdateDto, bindingContext);
    }

    private void ConvertDtoToEntity(ProviderDto providerDto, ModelBindingContext ctx)
    {
        var emailResult = Email.From(providerDto.Email);
        var companyNameResult = CompanyName.From(providerDto.CompanyName);
        var phoneNumber = providerDto.PhoneNumber;
        var id = providerDto.ProviderId;

        if(!CheckIfResultsAreSuccessful(emailResult,companyNameResult,ctx))
            return;

        var providerResult = Provider.Create(
            id: id,
            companyName: companyNameResult.Value,
            phoneNumber: phoneNumber,
            email: emailResult.Value);

        if (!providerResult.IsFailed)
        {
            ctx.Result = ModelBindingResult.Success(providerResult.Value);
            return;
        }

        ctx.ModelState.AddModelError("Provider", providerResult.Errors.First().Message);
    }

    private void ConvertUpdateDtoToEntity(ProviderUpdateDto providerDto, ModelBindingContext ctx)
    {
        var emailResult = Email.From(providerDto.Email);
        var companyNameResult = CompanyName.From(providerDto.CompanyName);
        var phoneNumber = providerDto.PhoneNumber;

        if(!CheckIfResultsAreSuccessful(emailResult,companyNameResult,ctx))
            return;

        var provider = Provider.Create(
            companyName: companyNameResult.Value,
            phoneNumber: phoneNumber,
            email: emailResult.Value);

        ctx.Result = ModelBindingResult.Success(provider);
    }

    private bool CheckIfResultsAreSuccessful(
        Result<Email> emailResult, 
        Result<CompanyName> companyNameResult, 
        ModelBindingContext ctx)
    {
        
        if (emailResult.IsFailed)
            ctx.ModelState.AddModelError("Email", emailResult.Errors.First().Message);

        if (companyNameResult.IsFailed)
            ctx.ModelState.AddModelError("CompanyName", companyNameResult.Errors.First().Message);

        return ctx.ModelState.ErrorCount == 0;
    }
}