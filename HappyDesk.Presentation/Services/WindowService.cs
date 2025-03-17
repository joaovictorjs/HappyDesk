using System.Windows;
using HappyDesk.Domain.Interfaces;

namespace HappyDesk.Presentation.Services;

public class WindowService<TWindow>(Func<TWindow> windowFactory) : IWindowService<TWindow>
    where TWindow : Window
{
    private TWindow? _window;

    public IWindowService<TWindow> CreateWindow()
    {
        _window = windowFactory();
        return this;
    }

    public IWindowService<TWindow> ShowDialog(Action<bool>? resultCallback)
    {
        var result = _window?.ShowDialog();
        resultCallback?.Invoke(result ?? false);
        return this;
    }
}