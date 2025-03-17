using HappyDesk.Domain.Models;

namespace HappyDesk.Domain.Interfaces;

public interface IPreferencesService
{
    Task<PreferencesModel> GetFirst(CancellationToken cancellationToken);
    Task<bool> Update(PreferencesModel model, CancellationToken cancellationToken);
}