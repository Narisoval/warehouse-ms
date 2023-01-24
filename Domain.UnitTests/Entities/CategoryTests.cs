using Domain.Entities;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class CategoryTests
{
    [Fact]
    public void Should_ChangeName_When_Called()
    {
        //Arrange
        var initialName = "Shoses";
        var sut = Category.Create(Guid.NewGuid(), initialName);
        var nameToChangeTo = "Shoes";
        
        //Act
        sut.ChangeName(nameToChangeTo);
        
        //Assert
        sut.Name.Should().BeEquivalentTo(nameToChangeTo);
        sut.Name.Should().NotBe(initialName);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeNameIsCalledWithNull()
    {
        //Arrange
        var sut = Category.Create(Guid.NewGuid(), "Electronics");
        string? nameToChangeTo = null;
        
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeName(nameToChangeTo));
    }
}