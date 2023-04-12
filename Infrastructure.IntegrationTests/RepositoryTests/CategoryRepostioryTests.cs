using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests.RepositoryTests;

public class CategoryRepositoryTests : ProductRelatedEntityTestsBase<Category, ICategoryRepository>
{
    public CategoryRepositoryTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public override async Task Should_UpdateEntity_When_Called()
    {
        // Arrange
        var seededCategory = GetRandomSeededEntity();

        // Detach the seeded entity to avoid tracking issues in EF Core
        Context.Entry(seededCategory).State = EntityState.Detached;
        
        // Act
        var updatedCategory = Category.Create(
            seededCategory.Id, CategoryName.From("UpdatedCategory").Value).Value;

        var result = await Repository.Update(updatedCategory);
        
        await UnitOfWork.Complete(); 
        
        var fetchedCategory = await Repository.Get(seededCategory.Id);
        
        // Assert
        result.Should().BeTrue();
        fetchedCategory.Should().BeEquivalentTo(updatedCategory);
    }

    [Fact]
    public override async Task Should_ReturnFalse_When_UpdatingNonExistingEntity()
    {
        // Arrange
        var nonExistingCategory = Category.Create(
            Guid.NewGuid(), CategoryName.From("NonExistingCategory").Value).Value;

        // Act
        var result = await Repository.Update(nonExistingCategory);
        await UnitOfWork.Complete();
        var fetchedNonExistentCategory = await Repository.Get(nonExistingCategory.Id);
        
        // Assert
        result.Should().BeFalse();
        fetchedNonExistentCategory.Should().BeNull();
    }
    
    [Fact]
    public override async Task Should_NotChangeAnything_When_UpdatingEntityWithNoChanges()
    {
        // Arrange
        var seededCategory = GetRandomSeededEntity();
        
        Context.Entry(seededCategory).State = EntityState.Detached;
        
        // Act
        var result = await Repository.Update(seededCategory);
        
        await UnitOfWork.Complete(); 
        
        var fetchedCategory = await Repository.Get(seededCategory.Id);
        
        // Assert
        result.Should().BeTrue();
        fetchedCategory.Should().BeEquivalentTo(seededCategory);
    }
    
    [Fact]
    public async Task Should_RemoveAllChildren_When_DeletingParent()
    {
       //Arrange
       var parent = Category.Create(Guid.NewGuid(), CategoryName.From("Socks").Value).Value;
       var child1 = Category.Create(Guid.NewGuid(), CategoryName.From("Dogs").Value,parent.Id).Value;
       var child2 = Category.Create(Guid.NewGuid(), CategoryName.From("Cocks").Value,child1.Id).Value;
       
       await Context.AddRangeAsync(parent, child1, child2);
       await Context.SaveChangesAsync();
       
       //Act 
       var isParentRemoved = await Repository.Remove(parent.Id);
       await UnitOfWork.Complete();
       
       //Assert
       isParentRemoved.Should().BeTrue();
       var child1FromDb = await Repository.Get(child1.Id);
       var child2FromDb = await Repository.Get(child2.Id);
       child1FromDb.Should().BeNull();
       child2FromDb.Should().BeNull();
    }
}