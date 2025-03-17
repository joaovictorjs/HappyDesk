using System.Diagnostics;
using System.Net.Mail;
using System.Security.Cryptography;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Domain.Models;
using HappyDesk.Infrastructure.Exceptions;

namespace HappyDesk.Presentation.ViewModels;

public class LoginViewModel : BindableBase, ICloseWindowAction
{
    private readonly ICredentialsService _credentialsService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IBrowserService _browserService;
    private readonly ISessionStore _sessionStore;

    private string _email = string.Empty;

    private bool _isLoading = true;

    private string _password = string.Empty;

    private bool _storeCredentials = true;

    public LoginViewModel(
        ICredentialsService credentialsService,
        IMessageBoxService messageBoxService,
        IBrowserService browserService,
        ISessionStore sessionStore
    )
    {
        _credentialsService = credentialsService;
        _messageBoxService = messageBoxService;
        _browserService = browserService;
        _sessionStore = sessionStore;

        LoginCommand = new AsyncDelegateCommand(Login, CanLogin);

        OnStartup();
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool ShowForm => !IsLoading;

    public bool StoreCredentials
    {
        get => _storeCredentials;
        set => SetProperty(ref _storeCredentials, value);
    }

    public AsyncDelegateCommand LoginCommand { get; }

    private bool CanLogin()
    {
        return MailAddress.TryCreate(_email, out _) && !string.IsNullOrEmpty(_password);
    }

    private async Task Login(CancellationToken cancellationToken)
    {
        try
        {
            IsLoading = true;
            _sessionStore.Value = await _browserService.AutomateLogin(Email, Password);
            var credentials = new CredentialsModel
            {
                Code = 1,
                Email = _storeCredentials ? Email : string.Empty,
                Password = _storeCredentials ?Password : string.Empty,
            };
            await _credentialsService.Update(credentials, cancellationToken);
            ExecuteClose.Invoke(true);
        }
        catch (Exception e)
        {
            e.Handle(_messageBoxService.ShowError);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async void OnStartup()
    {
        try
        {
            var result = await _credentialsService.GetFirst(CancellationToken.None);
            Email = result.Email;
            Password = result.Password;
            IsLoading = false;

            if (!CanLogin())
                return;

            await Login(CancellationToken.None);
        }
        catch (FormatException) { }
        catch (CryptographicException) { }
        catch (Exception e)
        {
            e.Handle(_messageBoxService.ShowError);
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        LoginCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }

    public Action<bool?> ExecuteClose { get; set; }
}
