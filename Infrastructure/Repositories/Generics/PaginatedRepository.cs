using Domain.Primitives;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Generics;

public abstract class PaginatedRepository<TEntity, TContext> :
    Repository<TEntity, TContext>, IPaginatedRepository<TEntity>
    where TEntity : Entity
    where TContext : DbContext
{
    public PaginatedRepository(TContext context) : base(context) { }

    public async Task<(IEnumerable<TEntity>, int)> GetAll(int pageIndex = 1, int pageSize = 15)
    {
        var totalCount = await Context.Set<TEntity>().CountAsync();

        var entities = await Context.Set<TEntity>().AsNoTracking()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (entities, totalCount);
    }
}
