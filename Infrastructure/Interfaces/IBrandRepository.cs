using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IBrandRepository : IRepository<Brand>
{
    Task<Brand?> GetBrandWithProducts(Guid id);
    Task<IEnumerable<Brand>> GetAllBrandsWithProducts();
}