using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BrandRepository : Repository<Brand,WarehouseDbContext>,IBrandRepository
{
    public BrandRepository(WarehouseDbContext context) : base(context)
    {
    }

    public async Task<Brand?> GetBrandWithProducts(Guid id)
    {
        return await Context.Brands
            .AsNoTracking()
            .Include(brand => brand.Products)
            .Where(brand => brand.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Brand>> GetAllBrandsWithProducts()
    {
        return await Context.Brands.AsNoTracking()
            .Include(brand => brand.Products)
            .ToListAsync();
    }
}