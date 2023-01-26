using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class CategoriesFixture
{
   public static Category GetTestCategory()
   {
      return Category.Create(Guid.NewGuid(), CategoryName.From("Shoes"));
   }
}