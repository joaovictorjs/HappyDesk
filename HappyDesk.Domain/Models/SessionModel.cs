namespace HappyDesk.Domain.Models;

public class SessionModel
{
    public required string AspNetCookie { get; set; }
    public required string RequestToken { get; set; }
    public required string WebSocket { get; set; }
    public required string Email { get; set; }
}