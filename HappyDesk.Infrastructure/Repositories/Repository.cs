using System.Linq.Expressions;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HappyDesk.Infrastructure.Repositories;

public class Repository<TEntity>(IDbContextFactory<SqliteContext> sqliteFactory)
    : IRepository<TEntity>
    where TEntity : class, IEntity
{
    public async Task<IEnumerable<TEntity>> Where(
        Expression<Func<TEntity, bool>> query,
        CancellationToken cancellationToken
    )
    {
        await using var sqlite = await sqliteFactory.CreateDbContextAsync(cancellationToken);
        return await sqlite.Set<TEntity>().Where(query).ToListAsync(cancellationToken);
    }

    public async Task<int> Insert(TEntity entity, CancellationToken cancellationToken)
    {
        await using var sqlite = await sqliteFactory.CreateDbContextAsync(cancellationToken);
        await sqlite.Set<TEntity>().AddAsync(entity, cancellationToken);
        return await sqlite.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> Update(TEntity entity, CancellationToken cancellationToken)
    {
        await using var sqlite = await sqliteFactory.CreateDbContextAsync(cancellationToken);
        sqlite.Set<TEntity>().Update(entity);
        return await sqlite.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> Delete(TEntity entity, CancellationToken cancellationToken)
    {
        await using var sqlite = await sqliteFactory.CreateDbContextAsync(cancellationToken);
        return await sqlite.Set<TEntity>().ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<List<TEntity>> ToList(CancellationToken cancellationToken)
    {
        await using var sqlite = await sqliteFactory.CreateDbContextAsync(cancellationToken);
        return await sqlite.Set<TEntity>().ToListAsync(cancellationToken);
    }
}