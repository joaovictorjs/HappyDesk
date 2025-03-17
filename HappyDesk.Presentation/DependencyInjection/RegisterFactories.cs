using Autofac;
using HappyDesk.Infrastructure.Contexts;
using HappyDesk.Infrastructure.Factories;
using Microsoft.EntityFrameworkCore;

namespace HappyDesk.Presentation.DependencyInjection;

public partial class DependencyInjector
{
    private void RegisterFactories()
    {
        _builder
            .RegisterType<SqliteContextFactory>()
            .As<IDbContextFactory<SqliteContext>>()
            .SingleInstance();
    }
}