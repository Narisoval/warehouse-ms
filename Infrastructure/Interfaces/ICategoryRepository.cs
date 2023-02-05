using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetCategoryWithProducts(Guid id);
    Task<IEnumerable<Category>> GetAllCategoriesWithProducts();
}