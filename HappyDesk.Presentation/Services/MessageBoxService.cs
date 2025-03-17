using System.Windows;
using HappyDesk.Domain.Interfaces;
using MessageBox = System.Windows.MessageBox;

namespace HappyDesk.Presentation.Services;

public class MessageBoxService : IMessageBoxService
{
    public void ShowError(string message)
    {
        MessageBox.Show(message, "HappyDesk - Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void ShowInformation(string message)
    {
        MessageBox.Show(
            message,
            "HappyDesk - Informação",
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );
    }

    public void ShowWarning(string message)
    {
        MessageBox.Show(
            message,
            "HappyDesk - Aviso",
            MessageBoxButton.OK,
            MessageBoxImage.Exclamation
        );
    }

    public bool ShowConfirmation(string message)
    {
        return MessageBox.Show(
            message,
            "HappyDesk - Confirmação",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question
        ) == MessageBoxResult.Yes;
    }
}