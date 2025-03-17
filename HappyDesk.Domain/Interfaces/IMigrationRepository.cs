namespace HappyDesk.Domain.Interfaces;

public interface IMigrationRepository : IRepository
{
    Task Migrate(CancellationToken cancellationToken);
}