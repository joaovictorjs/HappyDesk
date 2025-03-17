using System.IO.Abstractions;
using HappyDesk.Domain.Constants;
using HappyDesk.Domain.Interfaces;

namespace HappyDesk.Infrastructure.Services;

public class DatabaseService(IFileSystem fileSystem, IMigrationRepository migrationRepository) : IDatabaseService
{
    public async Task EnsureDatabaseExists(CancellationToken cancellationToken)
    {
        if (fileSystem.File.Exists(Filenames.Database))
            return;

        await migrationRepository.Migrate(cancellationToken);
    }
}