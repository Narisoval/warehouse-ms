using Domain.Primitives;
using Infrastructure.Interfaces;

namespace Infrastructure.IntegrationTests.Helpers.Extensions;

public static class UnitOfWorkExtensions
{ 
    public static TRepository GetRepository<TRepository,TEntity>(this IUnitOfWork unitOfWork) 
    where TRepository : class, IRepository<TEntity>
    where TEntity : Entity
    {
        if (typeof(TRepository) == typeof(ICategoryRepository))
            return (unitOfWork.Categories as TRepository)!;

        if (typeof(TRepository) == typeof(IBrandRepository))
            return (unitOfWork.Brands as TRepository)!;

        if (typeof(TRepository) == typeof(IProviderRepository))
            return (unitOfWork.Providers as TRepository)!;

        if (typeof(TRepository) == typeof(IProductRepository))
            return (unitOfWork.Products as TRepository)!;
        
        throw new InvalidOperationException($"Invalid repository type: {typeof(TRepository).Name}");
    }
}
