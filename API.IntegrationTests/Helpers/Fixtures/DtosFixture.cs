using Warehouse.API.DTO.BrandDtos;
using Warehouse.API.DTO.CategoryDtos;
using Warehouse.API.DTO.ProductDtos;
using Warehouse.API.DTO.ProviderDtos;

namespace API.IntegrationTests.Helpers.Fixtures;

public static class DtosFixture
{
    public static readonly CategoryUpdateDto CategoryUpdateDto = new() { Name = "Test Category" };

    public static readonly CategoryUpdateDto InvalidCategoryUpdateDto = new() { Name = "*" };
    
    public static readonly ProviderUpdateDto ProviderUpdateDto = new()
    {
        CompanyName = "New Provider",
        PhoneNumber = "123-456-7890",
        Email = "newprovider@example.com"
    };
    
    public static readonly ProviderUpdateDto InvalidProviderUpdateDto = new()
    {
        CompanyName = "",
        PhoneNumber = ".",
        Email = ".com"
    };
    
    public static readonly BrandUpdateDto BrandUpdateDto = new()
    {
        Name = "New Brand",
        Image = "https://example.com/new-brand-image.jpg",
        Description = "New brand description",
    };

    public static readonly BrandUpdateDto InvalidBrandUpdateDto = new()
    {
        Name = "", 
        Image = "", 
        Description = "", 
    };

    public static readonly ProductUpdateDto ProductUpdateDto = new()
    {
        Name = "Sample Product",
        Quantity = 10,
        FullPrice = 100.00M,
        Description = "A sample product for testing, text text text",
        MainImage = "https://example.com/sample-main-image.jpg",
        Images = new List<string>
        {
            "https://example.com/sample-image-1.jpg",
            "https://example.com/sample-image-2.jpg"
        },
        Sale = 10.00M,
        IsActive = true,
        CategoryId = EntitiesFixture.Categories.First().Id,
        ProviderId = EntitiesFixture.Providers.First().Id,
        BrandId = EntitiesFixture.Brands.First().Id
    };
    
    public static readonly ProductUpdateDto InvalidProductUpdateDto = new()
    {
        Name = "",
        Quantity = -10,
        FullPrice = -9m,
        Description = "",
        MainImage = "this is not an image at all",
        Images = new List<string>
        {
            "this is not an image at all",
            "this is not an image at all"
        },
        Sale = 10000.00M,
        IsActive = true,
        CategoryId = Guid.NewGuid(),
        ProviderId = Guid.NewGuid(),
        BrandId =  Guid.NewGuid()
    };
    
    public static readonly ProductUpdateDto ProductUpdateWithWrongForeignKeys = new()
    {
        Name = "Sample Product",
        Quantity = 10,
        FullPrice = 100.00M,
        Description = "A sample product for testing, text text text",
        MainImage = "https://example.com/sample-main-image.jpg",
        Images = new List<string>
        {
            "https://example.com/sample-image-1.jpg",
            "https://example.com/sample-image-2.jpg"
        },
        Sale = 10.00M,
        IsActive = true,
        CategoryId = Guid.NewGuid(),
        ProviderId = Guid.NewGuid(), 
        BrandId =  Guid.NewGuid()
    };
}