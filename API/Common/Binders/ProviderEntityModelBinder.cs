using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Warehouse.API.DTO.ProviderDtos;

namespace Warehouse.API.Common.Binders;

public sealed class ProviderEntityModelBinder : BaseModelBinder<ProviderDto,ProviderUpdateDto>
{
    protected override void ConvertDtoToEntity(ProviderDto providerDto)
    {
        var emailResult = Email.From(providerDto.Email);
        var companyNameResult = CompanyName.From(providerDto.CompanyName);
        var phoneNumber = providerDto.PhoneNumber;
        var id = providerDto.Id;

        if(!CheckIfResultsAreSuccessful(emailResult,companyNameResult))
            return;

        var providerResult = Provider.Create(
            id: id,
            companyName: companyNameResult.Value,
            phoneNumber: phoneNumber,
            email: emailResult.Value);

        if (!CheckIfProviderIsCreatedSuccessfully(providerResult))
            return;

        BindingContext.ModelState.AddModelError("Provider", providerResult.Errors.First().Message);
    }

    protected override void ConvertUpdateDtoToEntity(ProviderUpdateDto providerDto)
    {
        var emailResult = Email.From(providerDto.Email);
        var companyNameResult = CompanyName.From(providerDto.CompanyName);
        var phoneNumber = providerDto.PhoneNumber;

        if(!CheckIfResultsAreSuccessful(emailResult,companyNameResult))
            return;

        var providerResult = Provider.Create(
            companyName: companyNameResult.Value,
            phoneNumber: phoneNumber,
            email: emailResult.Value);

        if (!CheckIfProviderIsCreatedSuccessfully(providerResult))
            return;
        
        BindingContext.ModelState.AddModelError("Provider", providerResult.Errors.First().Message);
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
        
        return providerResult.IsFailed;
    }
}