using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Domain.UnitTests.Generators;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class ProductModelTests
{
    [Theory]
    [MemberData(nameof(NumbersForArithmeticOperationsGenerator.GenerateNumbersForSum),
        MemberType = typeof(NumbersForArithmeticOperationsGenerator))]
    public void Should_SumNumbers_When_IncreaseQuantityBy(int initialQuantity, int increaseBy)
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct(initialQuantity);
        //Act
        sut.IncreaseQuantityBy(increaseBy);
        //Assert
        sut.Quantity.Value.Should().Be(initialQuantity + increaseBy);
    }

    [Theory]
    [MemberData(nameof(NumbersForArithmeticOperationsGenerator.GenerateNumbersForSubtraction),
        MemberType = typeof(NumbersForArithmeticOperationsGenerator))]
    public void Should_SubtractNumbers_When_DecreaseQuantityBy(int initialQuantity, int decreaseBy)
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct(initialQuantity);
        //Act
        sut.DecreaseQuantityBy(decreaseBy);
        //Assert
        sut.Quantity.Value.Should().Be(initialQuantity - decreaseBy);
    }

    [Fact]
    public void Should_ThrowException_When_IncreaseQuantityIsCalledWithNegativeNumber()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct(30);

        //Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.IncreaseQuantityBy(-6));
    }

    [Fact]
    public void Should_ChangeFullPrice_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();

        //Act 
        var newPrice = Price.From(3050);
        sut.ChangeFullPrice(newPrice);

        //Assert
        sut.FullPrice.Should().BeEquivalentTo(newPrice);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeFullPriceArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();

        //Act && Assert
        Price? newPrice = null;
        Assert.Throws<ArgumentNullException>(() => sut.ChangeFullPrice(newPrice));
    }

    [Fact]
    public void Should_ChangeImages_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var newImages = ProductImagesFixture.GetProductImages();

        //Act 
        sut.ChangeImages(newImages);

        //Assert
        sut.Images.Should().BeEquivalentTo(newImages);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeImagesArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        List<ProductImage>? newProductImages = null;
        //Act && Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeImages(newProductImages));
    }

    [Fact]
    public void Should_ThrowException_WhenChangeImageArgumentIsWrongId()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var newImageId = Guid.NewGuid();
        var newImage = Image.From("https://abc.png");
        
        //Act && Assert
        Assert.Throws<ArgumentException>(() => sut.ChangeImage(newImageId,newImage));  
    }

    [Fact]
    public void Should_ThrowException_When_ChangeImageIdIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var imageToChangeTo = Image.From("https://a.png");
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeImage(null,imageToChangeTo));
    }
    
    [Fact]
    public void Should_ThrowException_When_ChangeImageImageIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var imageToChangeTo = Image.From("https://a.png");
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeImage(Guid.NewGuid(),null));
    }

    [Fact]
    public void Should_ChangeRightImage_When_ChangeImageWhenCalled()
    {
        //Arrange
        var productImages = ProductImagesFixture.GetProductImages();
        var sut = ProductsFixture.GetTestProduct(productImages);
        var newImage = Image.From("https://abc.png");
        
        //Act
        foreach (var currProductImage in sut.Images)
        {
            var initialImage = currProductImage.Image;
            sut.ChangeImage(currProductImage.Id, newImage);
            
            //Assert
            currProductImage.Image.Should().BeEquivalentTo(newImage);
            
            currProductImage.Image = initialImage;
        }
    }

    [Fact]
    public void Should_NotChangeOtherImages_WhenChangeImageIsCalled()
    {
        //Arrange
        var productImages = ProductImagesFixture.GetProductImages();
        var sut = ProductsFixture.GetTestProduct(productImages);
        var newImage = Image.From("https://abc.png");
        
        foreach (var currProductImage in sut.Images)
        {
            var initialImage = currProductImage.Image;
            
            //Act
            sut.ChangeImage(currProductImage.Id,newImage);
            
            var otherImages = sut.Images.Where(image => image.Id != currProductImage.Id);
            foreach(var otherImage in otherImages)
            {
                //Assert
                otherImage.Image.Should().NotBeEquivalentTo(newImage);
            }
            currProductImage.Image = initialImage;
        }
    }
    
    [Fact]
    public void Should_ChangeDescription_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var newDescription =
            ProductDescription.From(
                "new better and more detailed description of a great productfds");

        //Act 
        sut.ChangeDescription(newDescription);

        //Assert
        sut.ProductDescription.Should().BeEquivalentTo(newDescription);
    }

    [Fact]
    public void Should_ThrowException_WhenChangeDescriptionArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        ProductDescription? newDescription = null;
        //Act && Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeDescription(newDescription));
    }

    [Fact]
    public void Should_SetIsActiveTrue_When_EnableProductIsCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct(false);

        //Act
        sut.EnableProduct();

        //Assert
        sut.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Should_SetIsActiveFalse_When_DisableProductIsCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct(true);

        //Act
        sut.DisableProduct();

        //Assert
        sut.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Should_ChangeSale_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var originalSale = sut.Sale;
        var newSale = Sale.From(37.9M);

        //Act 
        sut.ChangeSale(newSale);

        //Assert
        sut.Sale.Should().NotBe(originalSale);
        sut.Sale.Should().Be(newSale);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeSaleArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        Sale? newSale = null;

        //Act && Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeSale(newSale));
    }

    [Fact]
    public void Should_ChangeName_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var originalName = sut.ProductName;
        var newName = ProductName.From("New really detailed name");

        //Act 
        sut.ChangeName(newName);

        //Assert
        sut.ProductName.Should().Be(newName);
        sut.ProductName.Should().NotBe(originalName);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeNameArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();

        //Act && Assert
        ProductName? newName = null;
        Assert.Throws<ArgumentNullException>(() => sut.ChangeName(newName));
    }

    [Fact]
    public void Should_ChangeBrandAndId_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var originalBrandId = sut.BrandId;

        var newBrand = BrandsFixture.GetTestBrand();
        var newBrandId = newBrand.Id;

        //Act 
        sut.ChangeBrand(newBrand);

        //Assert
        sut.Brand.Should().Be(newBrand);
        sut.BrandId.Should().Be(newBrandId);
        sut.BrandId.Should().NotBe(originalBrandId);
    }
    
    [Fact]
    public void Should_ThrowException_When_ChangeBrandArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();

        //Act 
        Brand? brand = null;

        //Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeBrand(brand));
    }

    [Fact]
    public void Should_ChangeProvider_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var originalProviderId = sut.ProviderId;

        var newProvider = ProvidersFixture.GetTestProvider();
        var newProviderId = newProvider.Id;

        //Act 
        sut.ChangeProvider(newProvider);

        //Assert
        sut.Provider.Should().Be(newProvider);
        sut.ProviderId.Should().Be(newProviderId);
        sut.ProviderId.Should().NotBe(originalProviderId);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeProviderArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();

        //Act 
        Provider? newProvider = null;

        //Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeProvider(newProvider));
    }
}