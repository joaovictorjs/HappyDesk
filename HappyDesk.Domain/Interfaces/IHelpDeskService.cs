using HappyDesk.Domain.Enums;
using HappyDesk.Domain.Models;

namespace HappyDesk.Domain.Interfaces;

public interface IHelpDeskService
{
    Task<Status> GetStatus();
    Task SetStatus(Status status);
    Task<List<TicketModel>> GetTickets();
}