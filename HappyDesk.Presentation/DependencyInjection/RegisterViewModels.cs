using Autofac;
using HappyDesk.Presentation.ViewModels;

namespace HappyDesk.Presentation.DependencyInjection;

public partial class DependencyInjector
{
    private void RegisterViewModels()
    {
        _builder.RegisterType<TrayViewModel>();

        _builder.RegisterType<MonitoredPeopleViewModel>();

        _builder.RegisterType<LoginViewModel>();
    }
}