using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using FluentResults;

namespace Domain.UnitTests.Entities;

public class BrandTests
{
    [Fact]
    public void Should_Create_Brand_When_All_Properties_Are_Provided()
    {
        //Arrange
        var id = Guid.NewGuid();
        var brandName = BrandName.From("BrandName").Value;
        var brandImage = Image.From("https://image.jpg").Value;
        var brandDescription = BrandDescription.From("This is a brand description").Value;
    
        //Act
        var brand = Brand.Create(id, brandName, brandImage, brandDescription).Value;
    
        //Assert
        Assert.Equal(id, brand.Id);
        Assert.Equal(brandName, brand.Name);
        Assert.Equal(brandImage, brand.Image);
        Assert.Equal(brandDescription, brand.Description);
    }

    [Fact]
    public void Should_ReturnFailedResult_When_SomeOfTheConstructorArgumentsAreNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var brandName = BrandName.From("BrandName").Value;
        var image = Image.From("https://image.jpg").Value;
        var description = BrandDescription.From("This is a brand description").Value;
        var arguments = new List<object?>() { brandName, image, description };

        // Act and Assert
        for (int i = 0; i < arguments.Count; i++)
        {
            var argument = arguments[i];
            arguments[i] = null;
            CreateBrand(id, arguments).IsFailed.Should().BeTrue();
            arguments[i] = argument;
        }
    }


    private Result<Brand> CreateBrand(Guid id, List<object?> arguments)
    {
        return Brand.Create(id,(BrandName)arguments[0]!,(Image)arguments[1]!,
            (BrandDescription)arguments[2]!);
    }
    
    
}