using Autofac;

namespace HappyDesk.Presentation.DependencyInjection;

public partial class DependencyInjector
{
    private static DependencyInjector? _instance;
    private readonly ContainerBuilder _builder = new();
    private readonly IContainer _container;

    private DependencyInjector()
    {
        MakeRegistrations();
        _container = _builder.Build();
    }

    public DependencyInjector(IContainer container)
    {
        _container = container;
    }

    public static DependencyInjector Instance => _instance ??= new DependencyInjector();

    private void MakeRegistrations()
    {
        RegisterFactories();
        RegisterRepositories();
        RegisterServices();
        RegisterViewModels();
        RegisterViews();
        RegisterStores();
    }

    public T Resolve<T>() where T : notnull
    {
        return _container.Resolve<T>();
    }

    public object Resolve(Type type)
    {
        return _container.Resolve(type);
    }
}