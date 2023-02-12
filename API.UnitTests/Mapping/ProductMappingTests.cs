using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO;

namespace API.UnitTests.Mapping;

public class ProductMappingTests
{
    private readonly Product _product;
    private readonly ProductDto _productDto;
    
    public ProductMappingTests()
    {
        _product = ProductsFixture.GetTestProduct();
        _productDto = new ProductDto
        {
            ProductId = _product.Id,
            Name = _product.Name.Value,
            Description = _product.Description.Value,
            Quantity = _product.Quantity.Value,
            FullPrice = _product.FullPrice.Value,
            Sale = _product.Sale.Value,
            IsActive = _product.IsActive,
            Category = _product.Category!.ToDto(),
            Brand = _product.Brand!.ToDto(),
            Images = _product.Images.ToDtos(),
            Provider = _product.Provider!.ToDto()
        };
    }
    
    [Fact]
    public void Should_MapProductToProductDto_When_ProductIsValid()
    {

        var mappedProductDto = _product.ToDto();

        Assert.Equal(_product.Id, mappedProductDto.ProductId);
        Assert.Equal(_product.Name.Value, mappedProductDto.Name);
        Assert.Equal(_product.Quantity.Value, mappedProductDto.Quantity);
        Assert.Equal(_product.FullPrice.Value, mappedProductDto.FullPrice);
        Assert.Equal(_product.Description.Value, mappedProductDto.Description);
        Assert.Equal(_product.Sale.Value, mappedProductDto.Sale);
        Assert.Equal(_product.IsActive, mappedProductDto.IsActive);


        CheckCategoryMappedCorrectly(mappedProductDto.Category!);
        CheckProviderMappedCorrectly(mappedProductDto.Provider!);
        CheckBrandMappedCorrectly(mappedProductDto.Brand!);
        CheckProductImagesMappedCorrectly(mappedProductDto.Images!);
    }
    
    [Fact]
    public void Should_MapProductDtoToProduct_When_ProductDtoIsValid()
    {

        var mappedProduct = _productDto.ToEntity();

        Assert.Equal(_productDto.ProductId, mappedProduct.Id);
        Assert.Equal(_productDto.Name, mappedProduct.Name.Value);
        Assert.Equal(_productDto.Quantity, mappedProduct.Quantity.Value);
        Assert.Equal(_productDto.FullPrice, mappedProduct.FullPrice.Value);
        Assert.Equal(_productDto.Description, mappedProduct.Description.Value);
        Assert.Equal(_productDto.Sale, mappedProduct.Sale.Value);
        Assert.Equal(_productDto.IsActive, mappedProduct.IsActive);

        CheckCategoryMappedCorrectly(_productDto.Category!, mappedProduct.Category!);
        CheckProviderMappedCorrectly(_productDto.Provider!, mappedProduct.Provider!);
        CheckBrandMappedCorrectly(_productDto.Brand!, mappedProduct.Brand!);
        CheckProductImagesMappedCorrectly(_productDto.Images!, mappedProduct.Images!);
    }

    private void CheckProviderMappedCorrectly(ProviderDto provider)
    {
        Assert.NotNull(provider);
        Assert.Equal(_product.Provider.Id, provider.ProviderId);
        Assert.Equal(_product.Provider.CompanyName.Value, provider.CompanyName);
    }

    private void CheckCategoryMappedCorrectly(CategoryDto category)
    {
        Assert.NotNull(category);
        Assert.Equal(_product.Category.Id, category.CategoryId);
        Assert.Equal(_product.Category.Name.Value,category.Name);
    }

    private void CheckBrandMappedCorrectly(BrandDto brand)
    {
        Assert.Equal(brand.BrandId, _product.Brand.Id);
        Assert.Equal(brand.Name, _product.Brand.Name.Value);
        Assert.Equal(brand.Image, _product.Brand.Image.Value);
        Assert.Equal(brand.Description, _product.Brand.Description.Value);
    }

    private void CheckProductImagesMappedCorrectly(IList<ProductImageDto> productImages)
    {
        Assert.Equal(_product.Images.Count, productImages.Count);
        for(int i =0; i < _product.Images.Count; i++)
        {
            Assert.Equal(_product.Images[i].Id, productImages[i].ProductImageId);
            Assert.Equal(_product.Images[i].Image.Value, productImages[i].Image);
            Assert.Equal(_product.Images[i].IsMain, productImages[i].IsMain);
        }
    }

    private void CheckProviderMappedCorrectly(ProviderDto providerDto, Provider provider)
    {
        Assert.NotNull(provider);
        Assert.Equal(providerDto.ProviderId, provider.Id);
        Assert.Equal(providerDto.CompanyName, provider.CompanyName.Value);
    }

    private void CheckCategoryMappedCorrectly(CategoryDto categoryDto, Category category)
    {
        Assert.NotNull(category);
        Assert.Equal(categoryDto.CategoryId, category.Id);
        Assert.Equal(categoryDto.Name, category.Name.Value);
    }

    private void CheckBrandMappedCorrectly(BrandDto brandDto, Brand brand)
    {
        Assert.Equal(brandDto.BrandId, brand.Id);
        Assert.Equal(brandDto.Name, brand.Name.Value);
        Assert.Equal(brandDto.Image, brand.Image.Value);
        Assert.Equal(brandDto.Description, brand.Description.Value);
    }

    private void CheckProductImagesMappedCorrectly(IList<ProductImageDto> productImageDtos, IList<ProductImage> productImages)
    {
        Assert.Equal(productImageDtos.Count, productImages.Count);
        for (int i = 0; i < productImageDtos.Count; i++)
        {
            Assert.Equal(productImageDtos[i].ProductImageId, productImages[i].Id);
            Assert.Equal(productImageDtos[i].Image, productImages[i].Image.Value);
            Assert.Equal(productImageDtos[i].IsMain, productImages[i].IsMain);
        }
    } 
}