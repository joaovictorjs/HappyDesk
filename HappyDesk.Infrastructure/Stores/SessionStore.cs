using HappyDesk.Domain.Interfaces;
using HappyDesk.Domain.Models;
using static System.String;

namespace HappyDesk.Infrastructure.Stores;

public class SessionStore:ISessionStore
{
    public SessionModel Value { get; set; } = new SessionModel
    {
        AspNetCookie = Empty,
        RequestToken = Empty,
        WebSocket = Empty,
        Email = Empty
    };
}