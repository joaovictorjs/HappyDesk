using HappyDesk.Domain.Contants;
using HappyDesk.Domain.Interfaces;

namespace HappyDesk.Presentation.ViewModels;

public class TrayViewModel
{
    private IApplicationService _applicationService;

    private readonly ToolStripMenuItem _notifyPreference;
    private readonly ToolStripMenuItem _loginPreference;
    private readonly ToolStripMenuItem _ticketMonitoringPreference;
    private readonly ToolStripMenuItem _loginLogout;
    private readonly ToolStripMenuItem _shutdown;
    private readonly ToolStripMenuItem _preferencesMenu;
    private readonly ToolStripMenuItem _statusMenu;
    private readonly ToolStripMenuItem _onlineStatus;
    private readonly ToolStripMenuItem _callingStatus;
    private readonly ToolStripMenuItem _inServiceStatus;
    private readonly ToolStripMenuItem _absentStatus;
    private readonly ToolStripMenuItem _offlineStatus;
    private readonly NotifyIcon _tray;

    public TrayViewModel(IApplicationService applicationService)
    {
        _applicationService = applicationService;

        _notifyPreference = new ToolStripMenuItem
        {
            CheckOnClick = true,
            Text = "Notificações",
            ToolTipText = "Habilitar/Desabilitar as notificações",
        };

        _loginPreference = new ToolStripMenuItem
        {
            CheckOnClick = true,
            Text = "Fazer login ao abrir",
            ToolTipText = "Habilitar/Desabilitar login ao abrir",
        };

        _ticketMonitoringPreference = new ToolStripMenuItem
        {
            Text = "Monitorar tickets abertos para",
            ToolTipText = "Abrir janela para configurar o monitoramento de tickets",
        };

        _preferencesMenu = new ToolStripMenuItem
        {
            Text = "Preferências",
            DropDownItems = { _notifyPreference, _loginPreference, _ticketMonitoringPreference },
        };

        _onlineStatus = new ToolStripMenuItem
        {
            CheckOnClick = true,
            Text = "Online",
            ToolTipText = "Mudar status do HelpDesk para Online",
        };

        _inServiceStatus = new ToolStripMenuItem
        {
            CheckOnClick = true,
            Text = "Em Atendimento",
            ToolTipText = "Mudar status do HelpDesk para Em Atendimento",
        };

        _absentStatus = new ToolStripMenuItem
        {
            CheckOnClick = true,
            Text = "Ausente",
            ToolTipText = "Mudar status do HelpDesk para Ausente",
        };

        _offlineStatus = new ToolStripMenuItem
        {
            CheckOnClick = true,
            Text = "Offline",
            ToolTipText = "Mudar status do HelpDesk para Offline",
        };

        _statusMenu = new ToolStripMenuItem
        {
            Text = "Status",
            DropDownItems = { _onlineStatus, _inServiceStatus, _absentStatus, _offlineStatus },
        };

        _loginLogout = new ToolStripMenuItem
        {
            Text = "Executar login",
            ToolTipText = "Executar login em sua conta HelpDesk",
        };

        _shutdown = new ToolStripMenuItem
        {
            Text = "Encerrar",
            ToolTipText = "Fechar completamente o HappyDesk",
            TextAlign = ContentAlignment.MiddleCenter
        };

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
                    new ToolStripSeparator(),
                    _shutdown,
                },
            },
        };
    }
}
