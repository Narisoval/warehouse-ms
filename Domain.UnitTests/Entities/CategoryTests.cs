using Domain.Entities;

namespace Domain.UnitTests.Entities;

public class CategoryTests
{
    [Theory]
    [InlineData("Shoes")]
    [InlineData("Clothes")]
    [InlineData("Electronics")]
    public void Should_CreateCategory_When_NameIsProvided(string name)
    {
        //Arrange
        var id = Guid.NewGuid();
        
        //Act
        var category = Category.Create(id, name);
        
        //Assert
        Assert.Equal(id, category.Id);
        Assert.Equal(name, category.Name);
    }

    [Fact]
    public void Should_ThrowArgumentNullException_When_NameIsNotProvided()
    {
        //Arrange
        var id = Guid.NewGuid();
        string name = null;
        
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => Category.Create(id, name));
    }
}
