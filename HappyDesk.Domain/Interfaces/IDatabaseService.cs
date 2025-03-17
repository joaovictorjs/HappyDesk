namespace HappyDesk.Domain.Interfaces;

public interface IDatabaseService
{
    Task EnsureDatabaseExists(CancellationToken cancellationToken);
}