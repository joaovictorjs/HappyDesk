namespace HappyDesk.Domain.Interfaces;

public interface IWindowService<TWindow> where TWindow : class
{
    IWindowService<TWindow> CreateWindow();

    IWindowService<TWindow> ShowDialog(Action<bool>? resultCallback);
}