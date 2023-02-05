using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    //TODO: pagination
    Task<Product?> GetProductWithProvider(Guid id);
    Task<IEnumerable<Product>> GetAllProductsWithProvider();
}