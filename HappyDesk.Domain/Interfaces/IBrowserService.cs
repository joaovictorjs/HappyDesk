using HappyDesk.Domain.Models;

namespace HappyDesk.Domain.Interfaces;

public interface IBrowserService
{
    Task<SessionModel> AutomateLogin(string email, string password);
    Task PrepareBrowser();
    Task<string> ManipulatePage(string email, string password);
    Task CloseBrowser();
    Task CheckPageError();
    Task<(string token, string cookie)> GetCookies();
}