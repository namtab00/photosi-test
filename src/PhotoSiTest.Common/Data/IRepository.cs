using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace PhotoSiTest.Common.Data;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default);


    Task DeleteAsync(Guid id, CancellationToken ct = default);


    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken ct = default);


    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);


    Task<TEntity> GetByIdOrThrowAsync(Guid id, CancellationToken ct = default);


    Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken ct = default);


    Task UpdateAsync(TEntity entity, CancellationToken ct = default);
}
