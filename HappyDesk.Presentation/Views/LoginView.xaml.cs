using HappyDesk.Domain.Interfaces;
using MahApps.Metro.Controls;

namespace HappyDesk.Presentation.Views;

public partial class LoginView : MetroWindow
{
    public LoginView()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (DataContext is ICloseWindowAction closeWindowAction)
            closeWindowAction.ExecuteClose = result =>
            {
                DialogResult = result;
                Close();
            };
    }
}
