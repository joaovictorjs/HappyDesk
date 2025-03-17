using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using HappyDesk.Domain.Constants;
using HappyDesk.Domain.Enums;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Domain.Models;

namespace HappyDesk.Infrastructure.Services;

public class HelpDeskService(ISessionStore sessionStore) : IHelpDeskService
{
    private async Task WithHttpClient(Func<HttpClient, Task> callback)
    {
        var container = new CookieContainer();
        container.Add(
            new Cookie
            {
                Name = CookieNames.Request,
                Value = sessionStore.Value.RequestToken,
                Domain = EndPoints.Domain,
            }
        );
        container.Add(
            new Cookie
            {
                Name = CookieNames.AspNet,
                Value = sessionStore.Value.AspNetCookie,
                Domain = EndPoints.Domain,
            }
        );
        using var handler = new HttpClientHandler { CookieContainer = container };
        using var client = new HttpClient(handler) { BaseAddress = new Uri(EndPoints.Base) };
        await callback(client);
    }

    public async Task<Status> GetStatus()
    {
        var result = string.Empty;

        await WithHttpClient(async client =>
        {
            var payload = JsonSerializer.Serialize(new { email = sessionStore.Value.Email });
            var body = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("UsuarioStatus/BuscaUsuarioStatusEmail", body);
            response.EnsureSuccessStatusCode();
            var contentResponse = await response.Content.ReadAsStringAsync();
            var responseBody = JsonSerializer.Deserialize<dynamic>(contentResponse);
            result = responseBody?.GetProperty("situacao").GetString();
        });

        return result switch
        {
            "Online" => Status.Online,
            "Em Atendimento" => Status.InService,
            "Ausente" => Status.Absent,
            _ => Status.Offline,
        };
    }

    public async Task SetStatus(Status status)
    {
        await WithHttpClient(async client =>
        {
            var payload = JsonSerializer.Serialize(
                new { email = sessionStore.Value.Email, situacao = (int)status }
            );
            var body = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("UsuarioStatus/AtualizaUsuarioStatus", body);
            response.EnsureSuccessStatusCode();
        });
    }

    public async Task<List<TicketModel>> GetTickets()
    {
        var tickets = new List<TicketModel>();

        await WithHttpClient(async client =>
        {
            var response = await client.GetAsync("api/ticket");
            response.EnsureSuccessStatusCode();
            var contentResponse = await response.Content.ReadAsStringAsync();
            tickets = JsonSerializer.Deserialize<List<TicketModel>>(contentResponse);
        });

        return tickets;
    }
}
