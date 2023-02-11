using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Entities;

public class BrandTests
{
    [Fact]
    public void Should_Create_Brand_When_All_Properties_Are_Provided()
    {
        //Arrange
        var id = Guid.NewGuid();
        var brandName = BrandName.From("BrandName");
        var brandImage = Image.From("https://image.jpg");
        var brandDescription = BrandDescription.From("This is a brand description");
    
        //Act
        var brand = Brand.Create(id, brandName, brandImage, brandDescription);
    
        //Assert
        Assert.Equal(id, brand.Id);
        Assert.Equal(brandName, brand.Name);
        Assert.Equal(brandImage, brand.Image);
        Assert.Equal(brandDescription, brand.Description);
    }

    [Fact]
    public void Should_ThrowException_When_SomeOfTheConstructorArgumentsAreNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var brandName = BrandName.From("BrandName");
        var image = Image.From("https://image.jpg");
        var description = BrandDescription.From("This is a brand description");
        var validArguments = new List<object?>() { brandName, image, description };

        // Act and Assert
        for (int i = 0; i < validArguments.Count; i++)
        {
            var currValidArgument = validArguments[i];
            validArguments[i] = null;
            Assert.Throws<ArgumentNullException>(() => CreateBrand(id,validArguments));
            validArguments[i] = currValidArgument;
        }
    }


    private Brand CreateBrand(Guid id, List<object?> arguments)
    {
        return Brand.Create(id,(BrandName)arguments[0]!,(Image)arguments[1]!,
            (BrandDescription)arguments[2]!);
    }
    
    
}