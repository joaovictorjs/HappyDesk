using HappyDesk.Domain.Contants;
using HappyDesk.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HappyDesk.Infrastructure.Factories;

public class SqliteDesignTimeContextFactory : IDesignTimeDbContextFactory<SqliteContext>
{
    public SqliteContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<SqliteContext>();
        builder.UseSqlite($"Data Source={Filenames.Database};Version=3;");
        return new SqliteContext(builder.Options);
    }
}