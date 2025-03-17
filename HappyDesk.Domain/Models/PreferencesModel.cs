using HappyDesk.Domain.Entities;
using HappyDesk.Domain.Interfaces;

namespace HappyDesk.Domain.Models;

public class PreferencesModel : IModel
{
    public required int Code { get; set; }
    public required bool IsAutoLoginEnabled { get; set; }
    public required bool IsNotificationEnable { get; set; }
    public required string[] ObservedPeople { get; set; }

    public IEntity ToEntity()
    {
        return new PreferencesEntity
        {
            Code = Code,
            IsAutoLoginEnabled = IsAutoLoginEnabled,
            IsNotificationEnable = IsNotificationEnable,
            ObservedPeople = string.Join(
                ", ",
                ObservedPeople.Select(it => it.Trim()).Where(it => !string.IsNullOrEmpty(it))
            )
        };
    }
}