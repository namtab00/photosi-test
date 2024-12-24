using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PhotoSiTest.Common.Exceptions;

namespace PhotoSiTest.Common.Data;

public abstract class RepositoryBase<TEntity, TContext>(TContext context) : IRepository<TEntity>
    where TEntity : PhotoSiTestEntity
    where TContext : DbContext
{
    public TContext Context { get; } = context;

    protected DbSet<TEntity> Set => Context.Set<TEntity>();


    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default)
    {
        await Set.AddAsync(entity, ct);
        await Context.SaveChangesAsync(ct);
        return entity;
    }


    public virtual async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var user = await GetByIdAsync(id, ct);
        if (user != null)
        {
            Set.Remove(user);
            await Context.SaveChangesAsync(ct);
        }
    }


    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken ct = default)
    {
        var query = Set.AsQueryable().Where(filter);

        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(cancellationToken: ct);
    }


    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await Set.FindAsync([id], ct);
    }


    public virtual async Task<TEntity> GetByIdOrThrowAsync(Guid id, CancellationToken ct = default)
    {
        return await GetByIdAsync(id, ct) ?? throw new EntityNotFoundException<TEntity>(id);
    }


    public virtual async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken ct = default)
    {
        var query = Set.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }

        return await query.ToListAsync(cancellationToken: ct);
    }


    public virtual async Task UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync(ct);
    }
}
