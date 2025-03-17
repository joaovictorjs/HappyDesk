using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using HappyDesk.Domain.Constants;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Domain.Models;
using PuppeteerSharp;

namespace HappyDesk.Infrastructure.Services;

public class BrowserService : IBrowserService
{
    private readonly BrowserFetcher _browserFetcher = new();
    private IBrowser? _browser;
    private IPage? _page;

    public async Task<SessionModel> AutomateLogin(string email, string password)
    {
        try
        {
            await PrepareBrowser();
            var websocket = await ManipulatePage(email, password);
            var (token, cookie) = await GetCookies();

            return new SessionModel
            {
                AspNetCookie = cookie,
                RequestToken = token,
                WebSocket = websocket,
                Email = email,
            };
        }
        finally
        {
            await CloseBrowser();
        }
    }

    public async Task<(string token, string cookie)> GetCookies()
    {
        var cookies = await _page!.GetCookiesAsync();

        var token = cookies
            .Where(it => it.Name == CookieNames.Request)
            .Select(it => it.Value)
            .FirstOrDefault();

        var cookie = cookies
            .Where(it => it.Name == CookieNames.AspNet)
            .Select(it => it.Value)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(cookie))
            throw new InvalidOperationException("Erro ao obter cookies!");

        return (token, cookie);
    }

    public async Task PrepareBrowser()
    {
        if (!_browserFetcher.GetInstalledBrowsers().Any())
            await _browserFetcher.DownloadAsync();

        _browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
    }

    public async Task<string> ManipulatePage(string email, string password)
    {
        _page = await _browser!.NewPageAsync();

        await _page.GoToAsync(EndPoints.Login);

        var client = await _page.CreateCDPSessionAsync();
        await client.SendAsync("Network.enable");
        string? websocket = null;

        client.MessageReceived += (sender, args) =>
        {
            if (args.MessageID != "Network.webSocketCreated")
                return;

            websocket = args.MessageData.GetProperty("url").GetString();
        };

        await _page.TypeAsync("input[name='Email']", email);
        await _page.TypeAsync("input[name='Password']", password);
        await _page.ClickAsync("input[type='submit']");

        await Task.Delay(TimeSpan.FromSeconds(3));

        await CheckPageError();

        return websocket
            ?? throw new InvalidOperationException("Não foi possível obter o websocket!");
    }

    public async Task CheckPageError()
    {
        if ((await _page!.QuerySelectorAllAsync(".validation-summary-errors")).Length != 0)
            throw new InvalidOperationException("Tentativa de login inválida");
    }

    public async Task CloseBrowser()
    {
        if (_browser != null)
            await _browser.CloseAsync();
    }
}
