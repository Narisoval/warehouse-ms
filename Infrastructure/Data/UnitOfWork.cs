using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly WarehouseDbContext _context;

    public UnitOfWork(WarehouseDbContext context)
    {
        _context = context;
        Products = new ProductRepository(context);
        Brands = new BrandRepository(context);
        Categories = new CategoryRepository(context);
        Providers = new ProviderRepository(context);
    }

    public IProductRepository Products { get; }
    public IBrandRepository Brands { get; }
    public ICategoryRepository Categories { get; }
    public IProviderRepository Providers { get; }
    
    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}