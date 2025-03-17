using Autofac;
using HappyDesk.Presentation.Views;

namespace HappyDesk.Presentation.DependencyInjection;

public partial class DependencyInjector
{
    private void RegisterViews()
    {
        _builder.RegisterType<MonitoredPeopleView>();

        _builder.RegisterType<LoginView>();
    }
}