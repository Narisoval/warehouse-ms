using Domain.Primitives;
using FluentAssertions;

namespace Domain.UnitTests.Foundation;

public class EntityTests
{
    [Fact]
    public void Should_ReturnTrue_When_CompareEntitiesWithSameIds()
    {
        //Arrange
        var id = Guid.NewGuid();
        var entityA = new TestEntity(id);
        var entityB = new TestEntity(id);

        //Act
        var areEntitiesEqual = entityA == entityB;
        var areEntitiesEqualEqualsMethod = entityA.Equals(entityB);
        
        //Assert
        areEntitiesEqual.Should().BeTrue();
        areEntitiesEqualEqualsMethod.Should().BeTrue();
    }
    

    [Fact]
    public void Should_ReturnFalse_When_ComparingEntitiesWithDifferentIds()
    {
        //Arrange
        var entityA = new TestEntity(Guid.NewGuid());
        var entityB = new TestEntity(Guid.NewGuid());

        //Act
        var areEntitiesEqual = entityA == entityB;
        var areEntitiesEqualEqualsMethod = entityA.Equals(entityB);
        
        var areEntitiesNotEqual = entityA != entityB;
        
        //Assert
        areEntitiesEqual.Should().BeFalse();
        areEntitiesEqualEqualsMethod.Should().BeFalse();
        areEntitiesNotEqual.Should().BeTrue();
    }

    [Fact]
    public void Should_ReturnFalse_When_ComparingWithNull()
    {
        //Arrange
        var entityA = new TestEntity(Guid.NewGuid());
        TestEntity? entityB = null; 

        //Act
        var areEntitiesEqual = entityA == entityB;
        var areEntitiesEqualEqualsMethod = entityA.Equals(entityB);
        
        var areEntitiesNotEqual = entityA != entityB;
        
        //Assert
        areEntitiesEqual.Should().BeFalse();
        areEntitiesEqualEqualsMethod.Should().BeFalse();
        areEntitiesNotEqual.Should().BeTrue();
    }
    
    private class TestEntity : Entity
    {
        public TestEntity(Guid id) : base(id)
        {
            ;
        }
    } 
}