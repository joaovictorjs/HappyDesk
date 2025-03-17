namespace HappyDesk.Domain.Interfaces;

public interface IWebSocketService
{
    public event Action TicketUpdated;
    public event Action StatusUpdated;

    Task ConnectAsync(string webSocket, CancellationToken cancellationToken);

    Task DisconnectAsync(CancellationToken cancellationToken);
}
