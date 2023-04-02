using Domain.Entities;

namespace API.IntegrationTests.Helpers;

public static class ProductHelpers
{
    public static Product GetNewProductWithChangedForeignKeys(Product productWithValues, 
        Guid? providerId = null, Guid? brandId = null, Guid? categoryId = null)
    {
        var newForeignKeys = GetNewForeignKeys
        (defaultProviderId: productWithValues.ProviderId, 
            defaultBrandId: productWithValues.BrandId, 
            defaultCategoryId: productWithValues.CategoryId,
            providerId, brandId, categoryId);

        return Product.Create(
            id: Guid.NewGuid(),
            productName: productWithValues.Name,
            quantity: productWithValues.Quantity,
            fullPrice: productWithValues.FullPrice,
            mainImage: productWithValues.MainImage, 
            images: productWithValues.Images,
            productDescription: productWithValues.Description,
            isActive: productWithValues.IsActive,
            sale: productWithValues.Sale,
            providerId: newForeignKeys[0],
            brandId: newForeignKeys[1],
            categoryId: newForeignKeys[2]).Value;
    }

    public static Guid[] GetNewForeignKeys(Guid defaultProviderId, Guid defaultBrandId, Guid defaultCategoryId, 
        params Guid?[] foreignKeys)
    {
        var newForeignKeys = new [] {
            foreignKeys[0] ?? defaultProviderId,
            foreignKeys[1] ?? defaultBrandId,
            foreignKeys[2] ?? defaultCategoryId
        };

        return newForeignKeys;
    }
}