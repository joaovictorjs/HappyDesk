using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HappyDesk.Domain.Interfaces;

namespace HappyDesk.Infrastructure.Services;

public class WebSocketService : IWebSocketService
{
    private ClientWebSocket _client = new();

    public event Action? TicketUpdated;
    public event Action? StatusUpdated;

    public async Task ConnectAsync(string webSocket, CancellationToken cancellationToken)
    {
        await _client.ConnectAsync(new Uri(webSocket), cancellationToken);
        _ = ReceiveMessages(cancellationToken);
    }

    private async Task ReceiveMessages(CancellationToken cancellationToken)
    {
        while (_client.State == WebSocketState.Open)
        {
            var buffer = new byte[1024];
            var result = await _client.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                cancellationToken
            );

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            Debug.WriteLine($"{DateTime.Now} - {message}");

            if (message.Contains("updateTickets") && message.Contains("updateStatus"))
            {
                Debug.WriteLine($"{DateTime.Now} - Disparado TicketUpdated && StatusUpdated");
                TicketUpdated?.Invoke();
                StatusUpdated?.Invoke();
            }
            else if (message.Contains("updateTickets") || message.Contains("notifyNewTicket"))
            {
                Debug.WriteLine($"{DateTime.Now} - Disparado TicketUpdated");
                TicketUpdated?.Invoke();
            }
            else if (message.Contains("updateStatus"))
            {
                Debug.WriteLine($"{DateTime.Now} - Disparado StatusUpdated");
                StatusUpdated?.Invoke();
            }
        }
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken)
    {
        if (_client.State == WebSocketState.Open)
        {
            await _client.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Closing",
                cancellationToken
            );
        }
        _client.Dispose();
        _client = new ClientWebSocket();
    }
}
