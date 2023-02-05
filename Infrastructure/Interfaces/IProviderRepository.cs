using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IProviderRepository : IRepository<Provider>
{
    Task<Provider?> GetProviderWithProducts(Guid id);
    Task<IEnumerable<Provider>> GetAllProvidersWithProducts();
}