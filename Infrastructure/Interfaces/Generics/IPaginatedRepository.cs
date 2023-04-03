using Domain.Primitives;

namespace Infrastructure.Interfaces.Generics;

public interface IPaginatedRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    Task<(IEnumerable<TEntity>, int)> GetAll(int pageIndex = 1, int pageSize = 15);
}
