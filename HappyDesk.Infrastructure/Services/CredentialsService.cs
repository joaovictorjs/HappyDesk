using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using HappyDesk.Domain.Entities;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Domain.Models;

namespace HappyDesk.Infrastructure.Services;

public class CredentialsService(IRepository<CredentialsEntity> repository) : ICredentialsService
{
    [SuppressMessage("Interoperability", "CA1416:Validar a compatibilidade da plataforma")]
    public async Task<CredentialsModel> GetFirst(CancellationToken cancellationToken)
    {
        var result = (await repository.ToList(cancellationToken)).First();
        var passwordBytes = ProtectedData.Unprotect(
            Convert.FromBase64String(result.Password),
            null,
            DataProtectionScope.LocalMachine
        );
        result.Password = Encoding.UTF8.GetString(passwordBytes);
        return (CredentialsModel)result.ToModel();
    }

    [SuppressMessage("Interoperability", "CA1416:Validar a compatibilidade da plataforma")]
    public async Task<bool> Update(
        CredentialsModel credentials,
        CancellationToken cancellationToken
    )
    {
        var passwordBytes = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(credentials.Password),
            null,
            DataProtectionScope.LocalMachine
        );
        credentials.Password = Convert.ToBase64String(passwordBytes);
        return await repository.Update((CredentialsEntity)credentials.ToEntity(), cancellationToken)
               > 0;
    }
}