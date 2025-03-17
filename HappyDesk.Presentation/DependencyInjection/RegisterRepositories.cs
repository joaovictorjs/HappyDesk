using Autofac;
using HappyDesk.Domain.Entities;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Infrastructure.Repositories;

namespace HappyDesk.Presentation.DependencyInjection;

public partial class DependencyInjector
{
    private void RegisterRepositories()
    {
        _builder.RegisterType<MigrationRepository>().As<IMigrationRepository>();

        _builder.RegisterType<Repository<PreferencesEntity>>().As<IRepository<PreferencesEntity>>();

        _builder.RegisterType<Repository<CredentialsEntity>>().As<IRepository<CredentialsEntity>>();
    }
}