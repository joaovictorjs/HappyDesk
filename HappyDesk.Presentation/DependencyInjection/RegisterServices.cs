using System.IO.Abstractions;
using Autofac;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Infrastructure.Services;
using HappyDesk.Presentation.Services;
using HappyDesk.Presentation.Views;

namespace HappyDesk.Presentation.DependencyInjection;

public partial class DependencyInjector
{
    private void RegisterServices()
    {
        _builder
            .Register<DatabaseService>(it => new DatabaseService(
                new FileSystem(),
                it.Resolve<IMigrationRepository>()
            ))
            .As<IDatabaseService>();

        _builder.RegisterType<PreferencesService>().As<IPreferencesService>();

        _builder.RegisterType<ApplicationService>().As<IApplicationService>();

        _builder.RegisterType<MessageBoxService>().As<IMessageBoxService>().SingleInstance();

        _builder
            .RegisterType<WindowService<MonitoredPeopleView>>()
            .As<IWindowService<MonitoredPeopleView>>();

        _builder.RegisterType<WindowService<LoginView>>().As<IWindowService<LoginView>>();

        _builder.RegisterType<CredentialsService>().As<ICredentialsService>();

        _builder.RegisterType<BrowserService>().As<IBrowserService>();

        _builder.RegisterType<WebSocketService>().As<IWebSocketService>();

        _builder.RegisterType<HelpDeskService>().As<IHelpDeskService>();

        _builder.RegisterType<LoggerService>().As<ILoggerService>();
    }
}
