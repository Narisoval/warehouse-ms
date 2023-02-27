using System.Text.Json;
using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.Common.Bindings;
using Warehouse.API.DTO.ProviderDtos;

namespace Warehouse.API.Common.Mapping.Bindings;

public class ProviderEntityModelBinder : BaseModelBinder
{
    public override async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        BindingContext = bindingContext;
        
        if (!CheckIfContentTypeIsJson())
            return;

        try
        {
            await BindProviderEntityAsync();
        }
        catch (JsonException ex)
        {
            bindingContext.ModelState.AddModelError(
                "ObjectFormatError",
                $"{ex.InnerException?.Message} The following json element caused a problem: {ex.Path}");
        }
    }

    private async Task BindProviderEntityAsync()
    {
        // Check if the route has a guid "id" route parameter
        if (TryGetIdFromRoute(out var guidId)) 
        {
            if (guidId != null) await BindFromDtoAsync(guidId.Value);
            return;
        }
        
        await BindFromUpdateDtoAsync();
    }

    private async Task BindFromDtoAsync(Guid id)
    {
        ProviderDto providerDto = (await BindingContext.HttpContext.Request
            .ReadFromJsonAsync<ProviderDto>())!;

        providerDto.ProviderId = id;

        ConvertDtoToEntity(providerDto);
    }
    
    private async Task BindFromUpdateDtoAsync()
    {
        ProviderUpdateDto providerUpdateDto = (await BindingContext.HttpContext.Request
            .ReadFromJsonAsync<ProviderUpdateDto>())!;

        ConvertUpdateDtoToEntity(providerUpdateDto);
    }

    private void ConvertDtoToEntity(ProviderDto providerDto)
    {
        var emailResult = Email.From(providerDto.Email);
        var companyNameResult = CompanyName.From(providerDto.CompanyName);
        var phoneNumber = providerDto.PhoneNumber;
        var id = providerDto.ProviderId;

        if(!CheckIfResultsAreSuccessful(emailResult,companyNameResult))
            return;

        var providerResult = Provider.Create(
            id: id,
            companyName: companyNameResult.Value,
            phoneNumber: phoneNumber,
            email: emailResult.Value);

        if (!providerResult.IsFailed)
        {
            BindingContext.Result = ModelBindingResult.Success(providerResult.Value);
            return;
        }

        BindingContext.ModelState.AddModelError("Provider", providerResult.Errors.First().Message);
    }

    private void ConvertUpdateDtoToEntity(ProviderUpdateDto providerDto)
    {
        var emailResult = Email.From(providerDto.Email);
        var companyNameResult = CompanyName.From(providerDto.CompanyName);
        var phoneNumber = providerDto.PhoneNumber;

        if(!CheckIfResultsAreSuccessful(emailResult,companyNameResult))
            return;

        var provider = Provider.Create(
            companyName: companyNameResult.Value,
            phoneNumber: phoneNumber,
            email: emailResult.Value);

        BindingContext.Result = ModelBindingResult.Success(provider);
    }

    private bool CheckIfResultsAreSuccessful(
        Result<Email> emailResult, 
        Result<CompanyName> companyNameResult)
    {
        
        if (emailResult.IsFailed)
            AddModelErrors(emailResult,"Email");

        if (companyNameResult.IsFailed)
            AddModelErrors(companyNameResult,"CompanyName");

        return BindingContext.ModelState.ErrorCount == 0;
    }
}