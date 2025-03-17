using HappyDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HappyDesk.Infrastructure.Contexts;

public class SqliteContext(DbContextOptions<SqliteContext> options) : DbContext(options)
{
    public DbSet<PreferencesEntity> Preferences { get; set; }
    public DbSet<CredentialsEntity> Credentials { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PreferencesEntity>(entity =>
        {
            entity.HasData(
                new PreferencesEntity
                {
                    Code = 1,
                    IsAutoLoginEnabled = true,
                    IsNotificationEnable = true,
                    ObservedPeople = string.Empty
                }
            );
        });

        modelBuilder.Entity<CredentialsEntity>(entity =>
        {
            entity.HasData(
                new CredentialsEntity
                {
                    Code = 1,
                    Email = string.Empty,
                    Password = string.Empty
                }
            );
        });
    }
}