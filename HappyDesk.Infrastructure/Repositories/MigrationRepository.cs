using HappyDesk.Domain.Interfaces;
using HappyDesk.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HappyDesk.Infrastructure.Repositories;

public class MigrationRepository(IDbContextFactory<SqliteContext> sqliteContext) : IMigrationRepository
{
    public async Task Migrate(CancellationToken cancellationToken)
    {
        await using var sqlite = await sqliteContext.CreateDbContextAsync(cancellationToken);

        await sqlite.Database.MigrateAsync(cancellationToken);
    }
}