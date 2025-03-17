using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Domain.Models;

namespace HappyDesk.Domain.Entities;

[Table("credenciais")]
public class CredentialsEntity : IEntity
{
    [Column("codigo")] [Key] public required int Code { get; set; }

    [Column("email")] public required string Email { get; set; }

    [Column("password")] public required string Password { get; set; }

    public IModel ToModel()
    {
        return new CredentialsModel
        {
            Code = Code,
            Email = Email,
            Password = Password
        };
    }
}