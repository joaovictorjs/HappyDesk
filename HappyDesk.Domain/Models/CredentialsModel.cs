using HappyDesk.Domain.Entities;
using HappyDesk.Domain.Interfaces;

namespace HappyDesk.Domain.Models;

public class CredentialsModel : IModel
{
    public required int Code { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

    public IEntity ToEntity()
    {
        return new CredentialsEntity
        {
            Code = Code,
            Email = Email,
            Password = Password
        };
    }
}