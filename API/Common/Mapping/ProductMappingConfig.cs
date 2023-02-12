using Domain.Entities;
using Domain.ValueObjects;
using Warehouse.API.DTO;

namespace Warehouse.API.Common.Mapping;

public static class ProductMappingConfig 
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            ProductId = product.Id,
            Name = product.Name.Value,
            Quantity = product.Quantity.Value,
            FullPrice = product.FullPrice.Value,
            Description = product.Description.Value,
            Images = product.Images.ToDtos(),
            Sale = product.Sale.Value,
            IsActive = product.IsActive,
            Brand = product.Brand?.ToDto(),
            Category = product.Category?.ToDto(),
            Provider = product.Provider?.ToDto()
        };
    }
    
    public static Product ToEntity(this ProductDto productDto)
    {
        return Product.Create(
            productDto.ProductId,
            ProductName.From(productDto.Name),
            Quantity.From(productDto.Quantity),
            Price.From(productDto.FullPrice),
            ProductImages.From(productDto.Images.ToEntities()),
            ProductDescription.From(productDto.Description),
            productDto.IsActive,
            Sale.From(productDto.Sale),
            productDto.Provider?.ToEntity(),
            productDto.Brand?.ToEntity(),
            productDto.Category?.ToEntity() 
        );

    }
}