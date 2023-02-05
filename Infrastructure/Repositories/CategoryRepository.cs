using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : Repository<Category,WarehouseDbContext>, ICategoryRepository
{
    public CategoryRepository(WarehouseDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetCategoryWithProducts(Guid id)
    {
        return await Context.Categories
            .AsNoTracking()
            .Include(category => category.Products)
            .Where(category => category.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesWithProducts()
    {
        return await Context.Categories
            .AsNoTracking()
            .Include(category => category.Products)
            .ToListAsync();
    }
}