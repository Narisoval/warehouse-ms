using Domain.Entities;
using Warehouse.API.DTO.ProductDtos;

namespace Warehouse.API.Common.Mapping;

public static class ProductMapping
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
            Brand = product.Brand?.ToUpdateDto(),
            Category = product.Category?.Name.Value,
            Provider = product.Provider?.ToUpdateDto()
        };
    }
}