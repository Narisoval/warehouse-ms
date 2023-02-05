
namespace Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IBrandRepository Brands { get; }
    ICategoryRepository Categories { get; }
    IProviderRepository Providers { get; }

    Task<int> Complete();
}