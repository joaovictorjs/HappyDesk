using System.Linq.Expressions;

namespace HappyDesk.Domain.Interfaces;

public interface IRepository;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<IEnumerable<TEntity>> Where(
        Expression<Func<TEntity, bool>> query,
        CancellationToken cancellationToken
    );

    Task<int> Insert(TEntity entity, CancellationToken cancellationToken);
    Task<int> Update(TEntity entity, CancellationToken cancellationToken);
    Task<int> Delete(TEntity entity, CancellationToken cancellationToken);
    Task<List<TEntity>> ToList(CancellationToken cancellationToken);
}