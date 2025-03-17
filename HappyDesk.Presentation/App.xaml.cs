using System.Windows;
using System.Windows.Threading;
using HappyDesk.Domain.Constants;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Presentation.DependencyInjection;
using HappyDesk.Presentation.ViewModels;
using Application = System.Windows.Application;

namespace HappyDesk.Presentation;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly DependencyInjector _injector;

    public App()
    {
        _injector = DependencyInjector.Instance;
        DispatcherUnhandledException += HandleUnhandledExceptions;
        ViewModelLocationProvider.SetDefaultViewModelFactory(_injector.Resolve);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }

    private void HandleUnhandledExceptions(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var message = e.Exception.Message;
        message += e.Exception.InnerException?.Message.Insert(0, "\n\n");
        _injector.Resolve<IMessageBoxService>().ShowError(message);
        _injector.Resolve<ILoggerService>().Log(message);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _injector.Resolve<TrayViewModel>();
    }
}
