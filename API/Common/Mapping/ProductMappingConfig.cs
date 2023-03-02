using Domain.Entities;
using Domain.ValueObjects;
using Warehouse.API.DTO;
using Warehouse.API.DTO.Product;

namespace Warehouse.API.Common.Mapping;

public static class ProductMappingConfig
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name.Value,
            Quantity = product.Quantity.Value,
            FullPrice = product.FullPrice.Value,
            Description = product.Description.Value,
            Images = product.Images.ToDtos(),
            Sale = product.Sale.Value,
            IsActive = product.IsActive,
            Brand = product.Brand?.ToUpdateDto(),
            Category = product.Category?.Name.Value,
            Provider = product.Provider?.ToUpdateDto()
        };
    }

    public static Product ToEntity(this ProductUpdateDto productDto)
    {
        return Product.Create(
            ProductName.From(productDto.Name).Value,
            Quantity.From(productDto.Quantity).Value,
            Price.From(productDto.FullPrice).Value,
            productDto.Images == null ? null : ProductImages.From(productDto.Images?.ToEntities()).Value,
            ProductDescription.From(productDto.Description).Value,
            productDto.IsActive,
            Sale.From(productDto.Sale).Value,
            productDto.ProviderId,
            productDto.BrandId,
            productDto.CategoryId
        ).Value;
    }

    public static Product ToEntity(this ProductUpdateDto productDto, Guid id)
    {
        return Product.Create(
            id,
            ProductName.From(productDto.Name).Value,
            Quantity.From(productDto.Quantity).Value,
            Price.From(productDto.FullPrice).Value,
            productDto.Images == null ? null : ProductImages.From(productDto.Images?.ToEntities()).Value,
            ProductDescription.From(productDto.Description).Value,
            productDto.IsActive,
            Sale.From(productDto.Sale).Value,
            productDto.ProviderId,
            productDto.BrandId,
            productDto.CategoryId
        ).Value;
    }

    
}