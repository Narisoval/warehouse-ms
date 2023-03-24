using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public sealed class CategoryRepository : Repository<Category,WarehouseDbContext>, ICategoryRepository
{
    public CategoryRepository(WarehouseDbContext context) : base(context)
    {
        ;
    }
}