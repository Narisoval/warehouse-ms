using Domain.Primitives;
using FluentAssertions;
using Infrastructure.IntegrationTests.Helpers;
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Infrastructure.Interfaces;

namespace Infrastructure.IntegrationTests.RepositoryTests;

public abstract class RepositoryTestsBase<TEntity,TRepository> : IClassFixture<DatabaseFixture>
   where  TEntity : Entity 
   where TRepository : class, IRepository<TEntity>
{
   protected readonly IUnitOfWork UnitOfWork;
   protected readonly DataSeeder Seeder;
   protected readonly TRepository Repository;
   protected readonly IReadOnlyList<TEntity> TestEntities;
   
   public RepositoryTestsBase(DatabaseFixture fixture)
   {
      Seeder = fixture.DataSeeder;
      UnitOfWork = fixture.UnitOfWork;
      
      Repository = UnitOfWork.GetRepository<TRepository, TEntity>();
      TestEntities = Seeder.GetList<TEntity>();
   }

   public abstract Task Should_UpdateEntity_When_Called();
   
   public abstract Task Should_NotChangeAnything_When_UpdatingEntityWithNoChanges();
   
   public abstract Task Should_ReturnFalse_When_UpdatingNonExistingEntity();
   
   [Fact]
   public async Task Should_RetrieveEntityWithCorrectValues_When_GettingEntityById()
   {
      //Arrange
      var testEntity = GetRandomSeededEntity();
      
      //Act
      var entityFromDb = await Repository.Get(testEntity.Id);
      
      //Assert
      entityFromDb.Should().NotBeNull();
      entityFromDb.Should().BeEquivalentTo(testEntity);
   }
   
   [Fact]
   public async Task Should_ReturnNull_When_GettingEntityWithNonExistingId()
   {
      //Arrange
      var entityId = Guid.NewGuid();
      
      //Act
      var entityFromDb = await Repository.Get(entityId);
      
      //Assert
      entityFromDb.Should().BeNull();
   }
   
   [Fact]
   public async Task Should_RetrieveAllEntities_When_GetAllIsCalled()
   {
      //Arrange & Act
      var entitiesFromDb = await Repository.GetAll();
      var entitiesList = entitiesFromDb.ToList();
      
      //Assert
      entitiesList.Should().NotBeEmpty();
      entitiesList.Should().BeEquivalentTo(TestEntities);
   }

   [Fact]
   public async Task Should_ReturnEmptyCollection_WhenGettingAllEntitiesAndNonArePresent()
   {
      //Arrange
      await EnsureEntitiesAreDeleted(TestEntities.ToArray());
      
      //Act
      var entitiesFromDb = await Repository.GetAll();
      
      //Assert
      entitiesFromDb.Should().BeEmpty();
      
      //Verify
      await EnsureEntitiesExists(TestEntities.ToArray());
   }
   
   [Fact]
   public async Task Should_AddEntity_When_Called()
   {
      //Arrange
      var entityToAdd = GetRandomSeededEntity();
      await EnsureEntitiesAreDeleted(entityToAdd);

      //Act
      await Repository.Add(entityToAdd);
      await UnitOfWork.Complete();
      var entityFromDb = await Repository.Get(entityToAdd.Id);
      
      //Assert
      entityFromDb.Should().BeEquivalentTo(entityToAdd);
   }
   
   [Fact]
   public async Task Should_DeleteEntity_When_Called()
   {
      //Arrange
      var entityToDelete = GetRandomSeededEntity();
      await EnsureEntitiesExists(entityToDelete);
      
      //Act
      await Repository.Remove(entityToDelete.Id);
      await UnitOfWork.Complete();
      
      var categoryFromDb = await Repository.Get(entityToDelete.Id);
      
      //Assert
      categoryFromDb.Should().BeNull();
      
      //Verify
      await EnsureEntitiesExists(entityToDelete);
   }

   protected TEntity GetRandomSeededEntity()
   {
      var rnd = new Random();
      const int startIndex = 0; 
      
      var indexInFixture = rnd.Next(startIndex, TestEntities.Count);
      
      return  TestEntities[indexInFixture];
   }

   protected async Task EnsureEntitiesExists(params TEntity[] entities)
   {
      var databaseChanged = false;
      
      foreach (var entity in entities)
      {
         var entityExists = await EntityExists(entity);
         if (!entityExists)
         {
            await Repository.Add(entity);
            databaseChanged = true;
         }
      }
      
      if(databaseChanged)
         await UnitOfWork.Complete();
   }
   
   protected async Task EnsureEntitiesAreDeleted(params TEntity[] entities)
   {
      
      bool databaseChanged = false;
      foreach (var entity in entities)
      {
         var entityExists = await EntityExists(entity);
         if (entityExists)
         {
            await Repository.Remove(entity.Id);
            databaseChanged = true;
         }
      }
      
      if(databaseChanged)
         await UnitOfWork.Complete();
   }

   private async Task<bool> EntityExists(TEntity entity)
   {
      var entityFromDb = await Repository.Get(entity.Id);
      return entityFromDb != null;
   }
}