using System.Diagnostics;
using System.Windows.Input;
using HappyDesk.Domain.Constants;
using HappyDesk.Domain.Enums;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Domain.Models;
using HappyDesk.Infrastructure.Exceptions;
using HappyDesk.Presentation.Views;

namespace HappyDesk.Presentation.ViewModels;

public class TrayViewModel
{
    private readonly ToolStripMenuItem _absentStatus;
    private readonly IApplicationService _applicationService;
    private readonly ICredentialsService _credentialsService;
    private readonly IDatabaseService _databaseService;
    private readonly ToolStripMenuItem _inServiceStatus;
    private readonly ToolStripMenuItem _loginLogout;
    private readonly ToolStripMenuItem _loginPreference;
    private readonly IWindowService<LoginView> _loginWindowService;
    private readonly IWindowService<MonitoredPeopleView> _monitoredPeopleWindowService;
    private readonly IWebSocketService _webSocketService;
    private readonly ISessionStore _sessionStore;
    private readonly IHelpDeskService _helpDeskService;
    private readonly IMessageBoxService _messageBoxService;

    private readonly ToolStripMenuItem _notifyPreference;
    private readonly ToolStripMenuItem _offlineStatus;
    private readonly ToolStripMenuItem _onlineStatus;
    private readonly ToolStripMenuItem _preferencesMenu;
    private readonly IPreferencesService _preferencesService;
    private readonly ToolStripMenuItem _shutdown;
    private readonly ToolStripMenuItem _statusMenu;
    private readonly ToolStripMenuItem _ticketMonitoringPreference;
    private readonly NotifyIcon _tray;
    private PreferencesModel _preferences;
    private readonly List<int> _notifiedTickets = [];

    public TrayViewModel(
        IApplicationService applicationService,
        IDatabaseService databaseService,
        IPreferencesService preferencesService,
        IWindowService<MonitoredPeopleView> MonitoredPeopleWindowService,
        ICredentialsService credentialsService,
        IWindowService<LoginView> loginWindowService,
        ISessionStore sessionStore,
        IHelpDeskService helpDeskService,
        IWebSocketService webSocketService,
        IMessageBoxService messageBoxService
    )
    {
        _applicationService = applicationService;
        _databaseService = databaseService;
        _preferencesService = preferencesService;
        _monitoredPeopleWindowService = MonitoredPeopleWindowService;
        _credentialsService = credentialsService;
        _loginWindowService = loginWindowService;
        _sessionStore = sessionStore;
        _webSocketService = webSocketService;
        _helpDeskService = helpDeskService;
        _webSocketService = webSocketService;
        _messageBoxService = messageBoxService;

        TogglePreferencesCommand = new AsyncDelegateCommand(TogglePreferences);
        OpenMonitoredPeopleWindowCommand = new DelegateCommand(OpenMonitoredPeopleWindow);
        OpenLoginWindowCommand = new AsyncDelegateCommand(OpenLoginWindow);
        SetOnlineCommand = new AsyncDelegateCommand(SetOnline);
        SetInServiceCommand = new AsyncDelegateCommand(SetInService);
        SetAbsentCommand = new AsyncDelegateCommand(SetAbsent);
        SetOfflineCommand = new AsyncDelegateCommand(SetOffline);
        ShutdownCommand = new AsyncDelegateCommand(Shutdown);

        _notifyPreference = CreateItem(
            "Notificações",
            "Habilitar/Desabilitar as notificações",
            TogglePreferencesCommand
        );

        _loginPreference = CreateItem(
            "Fazer login ao abrir",
            "Habilitar/Desabilitar login ao abrir",
            TogglePreferencesCommand
        );

        _ticketMonitoringPreference = CreateItem(
            "Monitorar tickets abertos para",
            "Abrir janela para configurar o monitoramento de tickets",
            OpenMonitoredPeopleWindowCommand,
            false
        );

        _preferencesMenu = CreateMenu(
            "Preferências",
            _notifyPreference,
            _loginPreference,
            _ticketMonitoringPreference
        );

        _onlineStatus = CreateItem(
            "Online",
            "Mudar status do HelpDesk para Online",
            SetOnlineCommand
        );

        _inServiceStatus = CreateItem(
            "Em Atendimento",
            "Mudar status do HelpDesk para Em Atendimento",
            SetInServiceCommand
        );

        _absentStatus = CreateItem(
            "Ausente",
            "Mudar status do HelpDesk para Ausente",
            SetAbsentCommand
        );

        _offlineStatus = CreateItem(
            "Offline",
            "Mudar status do HelpDesk para Offline",
            SetOfflineCommand
        );

        _statusMenu = CreateMenu(
            "Status",
            _onlineStatus,
            _inServiceStatus,
            _absentStatus,
            _offlineStatus
        );

        _loginLogout = CreateItem(
            "Executar login",
            "Executar login em sua conta HelpDesk",
            OpenLoginWindowCommand,
            false
        );

        _shutdown = CreateItem(
            "Encerrar",
            "Fechar completamente o HappyDesk",
            ShutdownCommand,
            false
        );

        _tray = new NotifyIcon
        {
            Icon = _applicationService.GetIcon(Filenames.Icon) as Icon,
            Visible = true,
            Text = "HappyDesk",
            ContextMenuStrip = new ContextMenuStrip
            {
                Items =
                {
                    _preferencesMenu,
                    _statusMenu,
                    _loginLogout,
                    new ToolStripSeparator{Text = "Teste"},
                    _shutdown,
                },
            },
        };

        _statusMenu.Enabled = false;

        _ = Startup(CancellationToken.None);

        _webSocketService.StatusUpdated += async () =>
        {
            await OnUpdateStatusMessage();
        };

        _webSocketService.TicketUpdated += async () =>
        {
            await OnUpdateTicketMessage();
        };
    }

    private async Task Shutdown()
    {
        try
        {
            await _webSocketService.DisconnectAsync(CancellationToken.None);
            _applicationService.Shutdown();
        }
        catch (Exception e)
        {
            e.Handle(_messageBoxService.ShowError);
        }
    }

    private async Task SetStatus(ToolStripMenuItem item, bool sendRequest = true)
    {
        _offlineStatus.Checked = item == _offlineStatus;
        _absentStatus.Checked = item == _absentStatus;
        _inServiceStatus.Checked = item == _inServiceStatus;
        _onlineStatus.Checked = item == _onlineStatus;

        if (!sendRequest)
            return;

        await _helpDeskService.SetStatus(
            item == _onlineStatus ? Status.Online
            : item == _inServiceStatus ? Status.InService
            : item == _absentStatus ? Status.Absent
            : Status.Offline
        );
    }

    private async Task SetOffline()
    {
        await SetStatus(_offlineStatus);
    }

    private async Task SetAbsent()
    {
        await SetStatus(_absentStatus);
    }

    private async Task SetInService()
    {
        await SetStatus(_inServiceStatus);
    }

    private async Task SetOnline()
    {
        await SetStatus(_onlineStatus);
    }

    public AsyncDelegateCommand SetOnlineCommand { get; }
    public AsyncDelegateCommand SetInServiceCommand { get; }
    public AsyncDelegateCommand SetAbsentCommand { get; }
    public AsyncDelegateCommand SetOfflineCommand { get; }
    private AsyncDelegateCommand TogglePreferencesCommand { get; }
    private DelegateCommand OpenMonitoredPeopleWindowCommand { get; }
    private AsyncDelegateCommand OpenLoginWindowCommand { get; }
    private AsyncDelegateCommand ShutdownCommand { get; }

    private void OpenMonitoredPeopleWindow()
    {
        _monitoredPeopleWindowService
            .CreateWindow()
            .ShowDialog(result =>
            {
                if (result)
                    _ = LoadPreferences(CancellationToken.None);
            });
    }

    private async Task TogglePreferences(CancellationToken arg)
    {
        try
        {
            _preferences.IsNotificationEnable = _notifyPreference.Checked;
            _preferences.IsAutoLoginEnabled = _loginPreference.Checked;
            await _preferencesService.Update(_preferences, arg);
        }
        catch (Exception e)
        {
            e.Handle(_messageBoxService.ShowError);
        }
    }

    private static ToolStripMenuItem CreateItem(
        string text,
        string tooltip,
        ICommand? command = null,
        bool checkOnClick = true
    )
    {
        return new ToolStripMenuItem
        {
            Text = text,
            ToolTipText = tooltip,
            CheckOnClick = checkOnClick,
            Command = command,
        };
    }

    private static ToolStripMenuItem CreateMenu(string text, params ToolStripItem[] items)
    {
        var menu = new ToolStripMenuItem { Text = text };
        menu.DropDownItems.AddRange(items);
        return menu;
    }

    public async Task Startup(CancellationToken cancellationToken)
    {
        try
        {
            await _databaseService.EnsureDatabaseExists(cancellationToken);
            await LoadPreferences(cancellationToken);

            if (_preferences.IsAutoLoginEnabled)
                await OpenLoginWindow(cancellationToken);
        }
        catch (Exception e)
        {
            e.Handle(_messageBoxService.ShowError);
        }
    }

    private async Task OpenLoginWindow(CancellationToken cancellationToken)
    {
        try
        {
            var success = false;
            _statusMenu.Enabled = false;

            _loginWindowService.CreateWindow().ShowDialog(result => success = result);

            if (!success)
                return;

            await _webSocketService.DisconnectAsync(cancellationToken);
            await _webSocketService.ConnectAsync(_sessionStore.Value.WebSocket, cancellationToken);

            _tray.ShowBalloonTip(
                3,
                "Conexão aberta",
                "A aplicação continuará rodando em segundo plano!",
                ToolTipIcon.Info
            );

            _statusMenu.Enabled = true;

            await OnUpdateStatusMessage();
        }
        catch (Exception e)
        {
            e.Handle(_messageBoxService.ShowError);
        }
    }

    private async Task OnUpdateTicketMessage()
    {
        try
        {
            if (!_preferences.IsNotificationEnable)
                return;

            var observed = _preferences.ObservedPeople;
            var remoteTickets = await _helpDeskService.GetTickets();

            var common = (
                from person in observed
                from ticket in remoteTickets
                where person == ticket.Person
                where ticket.Status == "Aberto"
                where !_notifiedTickets.Contains(ticket.Code)
                select ticket
            ).ToList();

            common.ForEach(it =>
            {
                _tray.ShowBalloonTip(
                    3,
                    "Novo ticket",
                    $"Ticket aberto para {it.Person}",
                    ToolTipIcon.Info
                );
                _notifiedTickets.Add(it.Code);
                Debug.WriteLine($"{DateTime.Now} - Notificacao lancada");
            });
        }
        catch (Exception e)
        {
            e.Handle(_messageBoxService.ShowError);
        }
    }

    private async Task OnUpdateStatusMessage()
    {
        try
        {
            var status = await _helpDeskService.GetStatus();

            _offlineStatus.Checked = status is Status.Offline;
            _absentStatus.Checked = status is Status.Absent;
            _inServiceStatus.Checked = status is Status.InService;
            _onlineStatus.Checked = status is Status.Online;

            Debug.WriteLine($"{DateTime.Now} - StatusUpdated Recebido");
        }
        catch (Exception e)
        {
            e.Handle(_messageBoxService.ShowError);
        }
    }

    private async Task LoadPreferences(CancellationToken cancellationToken)
    {
        try
        {
            _preferences = await _preferencesService.GetFirst(cancellationToken);
            SetPreferences();
        }
        catch (Exception e)
        {
            e.Handle(_messageBoxService.ShowError);
        }
    }

    private void SetPreferences()
    {
        _loginPreference.Checked = _preferences.IsAutoLoginEnabled;
        _notifyPreference.Checked = _preferences.IsNotificationEnable;
    }
}
