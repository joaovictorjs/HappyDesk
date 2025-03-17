using System.Text.Json.Serialization;
using HappyDesk.Domain.Entities;
using HappyDesk.Domain.Interfaces;

namespace HappyDesk.Domain.Models;

public class TicketModel : IModel
{
    [JsonPropertyName("id")]
    public required int Code { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("atendente")]
    public required string Person { get; set; }

    public IEntity ToEntity()
    {
        throw new NotImplementedException();
    }
}
