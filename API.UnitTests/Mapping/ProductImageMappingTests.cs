using Domain.Entities;
using Domain.ValueObjects;
using Warehouse.API.Common.Mapping;
using Warehouse.API.DTO;
using Random = System.Random;

namespace API.UnitTests.Mapping;

public class ProductImageMappingTests
{
    private readonly ProductImage _productImage;
    private readonly ProductImageDto _productImageDto;
    
    private const int ListSize = 10;

    public ProductImageMappingTests()
    {
        _productImage = ProductImage.Create(Guid.NewGuid(), Image.From("https://testimage.png"), true);

        _productImageDto = new ProductImageDto
        {
            ProductImageId = _productImage.Id,
            Image = _productImage.Image.Value,
            IsMain = _productImage.IsMain
        };
    }

    [Fact]
    public void Should_MapProductImageToProductImageDto_When_ProductImageIsValid()
    {
        var mappedProductImageDto = _productImage.ToDTo();

        Assert.Equal(_productImage.Id, mappedProductImageDto.ProductImageId);
        Assert.Equal(_productImage.Image.Value, mappedProductImageDto.Image);
        Assert.Equal(_productImage.IsMain, mappedProductImageDto.IsMain);
    }

    [Fact]
    public void Should_MapProductImageDtoToProductImage_When_ProductImageDtoIsValid()
    {
        var mappedProductImage = _productImageDto.ToEntity();

        Assert.Equal(_productImageDto.ProductImageId, mappedProductImage.Id);
        Assert.Equal(_productImageDto.Image, mappedProductImage.Image.Value);
        Assert.Equal(_productImageDto.IsMain, mappedProductImage.IsMain);
    }

    [Fact]
    public void Should_MapListOfProductImagesToListOfProductImageDtos_When_ProductImagesAreValid()
    {
        var productImages = GetRandomProductImages(ListSize);

        var mappedProductImageDtos = productImages.ToDtos();

        Assert.Equal(productImages.Count, mappedProductImageDtos.Count);
        
        
        for (int i = 0; i < productImages.Count; i++)
        {
            var currentEntity = productImages[i];
            var currentDto = mappedProductImageDtos[i];
            Assert.Equal(currentDto.ProductImageId, currentEntity.Id);
            Assert.Equal(currentDto.Image, currentEntity.Image.Value);
            Assert.Equal(currentDto.IsMain, currentEntity.IsMain);
        }
    }

    [Fact]
    public void Should_MapListOfProductImageDtosToListOfProductImages_When_ProductImageDtosAreValid()
    {
        //Arrange
        var productImageDtos = GetRandomProductImageDtos(ListSize);

        //Act
        var mappedProductImages = productImageDtos.ToEntities();
        
        //Assert
        Assert.Equal(productImageDtos.Count, mappedProductImages.Count);
        
        for (int i = 0; i < productImageDtos.Count; i++)
        {
            var currentEntity = mappedProductImages[i];
            var currentDto = productImageDtos[i];
            Assert.Equal(currentDto.ProductImageId, currentEntity.Id);
            Assert.Equal(currentDto.Image, currentEntity.Image.Value);
            Assert.Equal(currentDto.IsMain, currentEntity.IsMain);
        }
    }
    
    private IList<ProductImage> GetRandomProductImages(int count)
    {
        Random rnd = new Random();
        
        var productImages = new List<ProductImage>();
        
        for (int i = 0; i < count; i++)
        {
            var currImage = ProductImage.Create(
                Guid.NewGuid(), 
                Image.From($"https://{Guid.NewGuid()}.png"),
                rnd.NextDouble() >= 0.5);
                
            productImages.Add(currImage);
        }

        return productImages;
    }

    private IList<ProductImageDto> GetRandomProductImageDtos(int count)
    {
        Random rnd = new Random();
        
        var productImages = new List<ProductImageDto>();
        
        for (int i = 0; i < count; i++)
        {
            var currImage = new ProductImageDto()
            {
                ProductImageId = Guid.NewGuid(), 
                Image = $"https://{Guid.NewGuid()}.png",
                IsMain = rnd.NextDouble() >= 0.5
            };
                
            productImages.Add(currImage);
        }

        return productImages;
    }
}