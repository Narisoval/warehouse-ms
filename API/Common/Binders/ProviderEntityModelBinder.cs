using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO.ProviderDtos;

namespace Warehouse.API.Common.Binders;

public sealed class ProviderEntityModelBinder : BaseModelBinder<ProviderUpdateDto>
{
    protected override void ConvertDtoToEntity(ProviderUpdateDto providerDto, Guid? id)
    {
        var emailResult = Email.From(providerDto.Email);
        var companyNameResult = CompanyName.From(providerDto.CompanyName);
        var phoneNumber = providerDto.PhoneNumber;

        if(!CheckIfResultsAreSuccessful(emailResult,companyNameResult))
            return;

        Result<Provider> providerResult;
        if (id != null)
        {
            providerResult = Provider.Create(
                id: id.Value,
                companyName: companyNameResult.Value,
                phoneNumber: phoneNumber,
                email: emailResult.Value);
        }
        else
        {
            providerResult = Provider.Create(
                companyName: companyNameResult.Value,
                phoneNumber: phoneNumber,
                email: emailResult.Value);
        }

        if (!CheckIfProviderIsCreatedSuccessfully(providerResult))
            return;

        BindingContext.Result = ModelBindingResult.Success(providerResult.Value);
    }

    private bool CheckIfResultsAreSuccessful(Result<Email> emailResult, 
        Result<CompanyName> companyNameResult)
    {
        if (emailResult.IsFailed)
            AddModelErrors(emailResult.Errors,"Email");

        if (companyNameResult.IsFailed)
            AddModelErrors(companyNameResult.Errors,"CompanyName");

        return BindingContext.ModelState.ErrorCount == 0;
    }

    private bool CheckIfProviderIsCreatedSuccessfully(Result<Provider> providerResult)
    {
        if (providerResult.IsFailed)
            AddModelErrors(providerResult.Errors,"Provider");
        
        return providerResult.IsSuccess;
    }
}