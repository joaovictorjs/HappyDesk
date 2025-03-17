using Autofac;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Infrastructure.Stores;

namespace HappyDesk.Presentation.DependencyInjection;

public partial class DependencyInjector
{
    private void RegisterStores()
    {
        _builder.RegisterType<SessionStore>().As<ISessionStore>().SingleInstance();
    }
}
