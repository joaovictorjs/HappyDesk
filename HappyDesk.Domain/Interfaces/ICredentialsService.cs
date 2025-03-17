using HappyDesk.Domain.Models;

namespace HappyDesk.Domain.Interfaces;

public interface ICredentialsService
{
    Task<CredentialsModel> GetFirst(CancellationToken cancellationToken);
    Task<bool> Update(CredentialsModel credentials, CancellationToken cancellationToken);
}