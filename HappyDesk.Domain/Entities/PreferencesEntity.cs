using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Domain.Models;

namespace HappyDesk.Domain.Entities;

[Table("preferencias")]
public class PreferencesEntity : IEntity
{
    [Column("codigo")] [Key] public required int Code { get; set; }

    [Column("auto_login_habilitado")] public required bool IsAutoLoginEnabled { get; set; }

    [Column("notificacao_habilitada")] public required bool IsNotificationEnable { get; set; }

    [Column("pessoas_observadas")] public required string ObservedPeople { get; set; }

    public IModel ToModel()
    {
        return new PreferencesModel
        {
            Code = Code,
            IsAutoLoginEnabled = IsAutoLoginEnabled,
            IsNotificationEnable = IsNotificationEnable,
            ObservedPeople = ObservedPeople
                .Split(", ")
                .Select(it => it.Trim())
                .Where(it => !string.IsNullOrEmpty(it))
                .ToArray()
        };
    }
}