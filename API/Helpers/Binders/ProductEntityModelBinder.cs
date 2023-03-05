using Domain.Entities;
using Domain.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Warehouse.API.DTO.ProductDtos;
using Warehouse.API.Helpers.Mapping;

namespace Warehouse.API.Helpers.Binders;
public sealed class ProductEntityModelBinder : BaseModelBinder<ProductUpdateDto>
{
    protected override void ConvertDtoToEntity(ProductUpdateDto dto, Guid? id)
    {
        var productNameResult = ProductName.From(dto.Name);
        var quantityResult = Quantity.From(dto.Quantity);
        var priceResult = Price.From(dto.FullPrice);
        var descriptionResult = ProductDescription.From(dto.Description);
        var imagesResult = dto.Images?.ToProductImagesResult();
        var saleResult = Sale.From(dto.Sale);
        
        if (!CheckIfResultsAreSuccessFull(productNameResult, quantityResult, 
                priceResult, descriptionResult, imagesResult, saleResult))
            return;

        Result<Product> productResult;
        if (id == null)
        {
            productResult = Product.Create(
                productName: productNameResult.Value,
                quantity: quantityResult.Value,
                fullPrice: priceResult.Value,
                images: imagesResult.Value,
                productDescription: descriptionResult.Value,
                isActive: dto.IsActive,
                sale: saleResult.Value,
                providerId: dto.ProviderId,
                brandId : dto.BrandId,
                categoryId : dto.CategoryId
            );
        }
        else
        {
             productResult = Product.Create(
                 id: id.Value,
                 productName: productNameResult.Value,
                 quantity: quantityResult.Value,
                 fullPrice: priceResult.Value,
                 images: imagesResult.Value,
                 productDescription: descriptionResult.Value,
                 isActive: dto.IsActive,
                 sale: saleResult.Value,
                 providerId: dto.ProviderId,
                 brandId : dto.BrandId,
                 categoryId : dto.CategoryId);           
        }

        if (!CheckIfProductIsCreatedSuccessfully(productResult))
            return;

        BindingContext.Result = ModelBindingResult.Success(productResult.Value);
    }

    private bool CheckIfResultsAreSuccessFull(Result<ProductName> productNameResult, Result<Quantity> quantityResult,
        Result<Price> priceResult, Result<ProductDescription> descriptionResult, 
        Result<ProductImages> imagesResult, Result<Sale> saleResult)
    {
        if(productNameResult.IsFailed)
            AddModelErrors(productNameResult.Errors,"ProductName");
        
        if(quantityResult.IsFailed)
            AddModelErrors(quantityResult.Errors,"Quantity");
        
        if(descriptionResult.IsFailed)
            AddModelErrors(descriptionResult.Errors,"ProductDescription");
        
        if(priceResult.IsFailed)
            AddModelErrors(priceResult.Errors,"Price");
        
        if(descriptionResult.IsFailed)
            AddModelErrors(descriptionResult.Errors,"ProductDescription");
        
        if(imagesResult.IsFailed)
            AddModelErrors(imagesResult.Errors,"ProductImages");
        
        if(saleResult.IsFailed)
            AddModelErrors(saleResult.Errors,"Sale");
        
        return BindingContext.ModelState.ErrorCount == 0;
    }

    private bool CheckIfProductIsCreatedSuccessfully(Result<Product> productResult)
    {
        if (productResult.IsFailed)
            AddModelErrors(productResult.Errors, "Product");

        return productResult.IsSuccess;
    }
}