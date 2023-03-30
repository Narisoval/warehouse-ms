using Domain.Primitives;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class Repository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : Entity 
    where TContext : DbContext
{
    protected readonly TContext Context;

    public Repository(TContext context)
    {
        Context = context;
    }

    public async Task<TEntity?> Get(Guid id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAll(int pageIndex = 0,int pageSize = 15)
    {
        return await Context.Set<TEntity>()
            .Skip((pageIndex-1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task Add(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }

    public async Task<bool> Update(TEntity entity)
    {
        var entityExists = await Context.Set<TEntity>().AnyAsync(e => e.Id == entity.Id);

        if (!entityExists)
        {
            return false;
        }

        Context.Set<TEntity>().Update(entity);
        return true;
    }

    public async Task<bool> Remove(Guid id)
    {
        var entityFromDb = await Get(id);

        if (entityFromDb == null)
            return false;

        Context.Set<TEntity>().Remove(entityFromDb);
        return true;
    }
}