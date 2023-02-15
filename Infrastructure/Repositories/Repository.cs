using System.Linq.Expressions;
using Domain.Primitives;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class Repository<TEntity,TContext> : IRepository<TEntity> where TEntity : Entity where TContext : DbContext
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

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().Where(predicate);
    }

    public async Task Add(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRange(IEnumerable<TEntity?> entities)
    {
        await Context.Set<TEntity>().AddRangeAsync(entities);
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

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().RemoveRange(entities);
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
