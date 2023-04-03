using Domain.Primitives;
using FluentAssertions;
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Infrastructure.Interfaces.Generics;

namespace Infrastructure.IntegrationTests.RepositoryTests;

public abstract class PaginatedRepositoryTestsBase<TEntity,TRepository> : 
   RepositoryTestsBase<TEntity,TRepository>,IClassFixture<DatabaseFixture>
   where  TEntity : Entity 
   where TRepository : class, IPaginatedRepository<TEntity>
{
   protected PaginatedRepositoryTestsBase(DatabaseFixture fixture) : base(fixture)
   {
   }
   
   [Fact]
   public async Task Should_RetrievePaginatedEntities_When_GetAllWithPaginationIsCalled()
   {
       // Arrange
       var pageIndex = 1;
       var pageSize = 2;

       // Act
       var (pagedEntities , totalRecords) = await Repository.GetAll(pageIndex, pageSize);
       var entitiesList = pagedEntities.ToList();

       // Assert
       entitiesList.Should().NotBeEmpty();
       entitiesList.Should().HaveCount(pageSize);
       totalRecords.Should().Be(TestEntities.Count);
   }
   
   [Fact]
   public async Task Should_ReturnEmptyCollection_WhenGettingAllEntitiesWithPaginationAndNonArePresent()
   {
       // Arrange
       await EnsureEntitiesAreDeleted(TestEntities.ToArray());
       var pageIndex = 1;
       var pageSize = 2;

       // Act
       var (pagedEntities, totalRecords) = await Repository.GetAll(pageIndex, pageSize);

       // Assert
       pagedEntities.Should().BeEmpty();
       totalRecords.Should().Be(0);

       // Verify
       await EnsureEntitiesExists(TestEntities.ToArray());
   }
   
   [Fact]
   public async Task Should_ReturnCorrectTotalRecords_When_GetAllWithPaginationIsCalled()
   {
       // Arrange
       const int pageIndex = 1;
       const int pageSize = 1;

       // Act
       var (_ , totalRecords) = await Repository.GetAll(pageIndex, pageSize);
       
       var (allEntities, _) = await Repository.GetAll(pageIndex, totalRecords);
       var allEntitiesList = allEntities.ToList();
       
       var (extraEntities, _) = await Repository.GetAll(pageIndex+1, totalRecords);
       
       // Assert
       totalRecords.Should().Be(TestEntities.Count);
       allEntitiesList.Should().NotBeEmpty();
       allEntitiesList.Should().HaveCount(totalRecords);
       extraEntities.Should().BeEmpty();
   }
}