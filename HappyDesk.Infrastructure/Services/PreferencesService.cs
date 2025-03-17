using HappyDesk.Domain.Entities;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Domain.Models;

namespace HappyDesk.Infrastructure.Services;

public class PreferencesService(IRepository<PreferencesEntity> repository) : IPreferencesService
{
    public async Task<PreferencesModel> GetFirst(CancellationToken cancellationToken)
    {
        return (PreferencesModel)(await repository.ToList(cancellationToken)).First().ToModel();
    }

    public async Task<bool> Update(PreferencesModel model, CancellationToken cancellationToken)
    {
        return await repository.Update((PreferencesEntity)model.ToEntity(), cancellationToken) > 0;
    }
}