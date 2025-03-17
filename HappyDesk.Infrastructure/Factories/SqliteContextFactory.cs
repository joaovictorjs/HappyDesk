using HappyDesk.Domain.Constants;
using HappyDesk.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HappyDesk.Infrastructure.Factories;

public class SqliteContextFactory : IDbContextFactory<SqliteContext>
{
    public SqliteContext CreateDbContext()
    {
        var builder = new DbContextOptionsBuilder<SqliteContext>();
        builder.UseSqlite($"Data Source={Filenames.Database}");
        return new SqliteContext(builder.Options);
    }
}