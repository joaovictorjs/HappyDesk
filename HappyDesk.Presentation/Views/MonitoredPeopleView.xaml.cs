using HappyDesk.Domain.Interfaces;
using MahApps.Metro.Controls;

namespace HappyDesk.Presentation.Views;

public partial class MonitoredPeopleView : MetroWindow
{
    public MonitoredPeopleView()
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