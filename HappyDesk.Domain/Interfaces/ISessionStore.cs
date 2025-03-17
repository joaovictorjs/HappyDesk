using HappyDesk.Domain.Models;

namespace HappyDesk.Domain.Interfaces;

public interface ISessionStore
{
    SessionModel Value { get; set; }
}