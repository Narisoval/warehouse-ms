using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class CategoryRepository : Repository<Category, WarehouseDbContext>, ICategoryRepository
{
    public CategoryRepository(WarehouseDbContext context) : base(context)
    {
        ;
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        const string query = @"
                WITH RECURSIVE CategoryHierarchy AS (
                    SELECT * FROM ""Categories"" WHERE ""ParentId"" IS NULL
                    UNION ALL
                    SELECT c.* FROM ""Categories"" c
                    INNER JOIN CategoryHierarchy ch ON c.""ParentId"" = ch.""Id""
                )
                SELECT * FROM CategoryHierarchy;
            ";

            // Execute the query and map the results to the Category entity
            var categories = await Context.Categories.FromSqlRaw(query).ToListAsync();

            // Return the root categories (categories with no parent)
            return categories.Where(c => c.ParentId == null).ToList();
    }
    
    public new async Task<Category?> Get(Guid id)
    {
        const string query = @"
        WITH RECURSIVE CategoryHierarchy AS (
            SELECT * FROM ""Categories"" WHERE ""Id"" = @p0
            UNION ALL
            SELECT c.* FROM ""Categories"" c
            INNER JOIN CategoryHierarchy ch ON c.""ParentId"" = ch.""Id""
        )
        SELECT * FROM CategoryHierarchy;
    ";

        // Execute the query and map the results to the Category entity
        var categories = await Context.Set<Category>().FromSqlRaw(query, id).ToListAsync();

        // Return the root category
        return categories.FirstOrDefault(c => c.Id == id);
    }

}