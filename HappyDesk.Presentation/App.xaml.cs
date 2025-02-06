using HappyDesk.Presentation.ViewModels;
using Application = System.Windows.Application;

namespace HappyDesk.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        var trayVm = new TrayViewModel();
    }
}