using System.Collections.ObjectModel;
using HappyDesk.Domain.Interfaces;
using HappyDesk.Domain.Models;
using HappyDesk.Infrastructure.Exceptions;

namespace HappyDesk.Presentation.ViewModels;

public class MonitoredPeopleViewModel : BindableBase, ICloseWindowAction
{
    private readonly IMessageBoxService _messageBoxService;

    private readonly IPreferencesService _preferencesService;

    private int _currentIndex = -1;

    private string _personName = string.Empty;
    private PreferencesModel _preferences;

    public MonitoredPeopleViewModel(
        IPreferencesService preferencesService,
        IMessageBoxService messageBoxService
    )
    {
        _preferencesService = preferencesService;
        _messageBoxService = messageBoxService;

        _ = OnStartup();

        AddPersonCommand = new DelegateCommand(AddPerson, CanAddPerson);
        RemovePersonCommand = new DelegateCommand(RemovePerson, CanRemovePerson);
        SaveChangesCommand = new AsyncDelegateCommand(SaveChanges);
    }

    public ObservableCollection<string> ObservedPeople { get; } = [];

    public string PersonName
    {
        get => _personName;
        set => SetProperty(ref _personName, value);
    }

    public int CurrentIndex
    {
        get => _currentIndex;
        set => SetProperty(ref _currentIndex, value);
    }

    public AsyncDelegateCommand SaveChangesCommand { get; }
    public DelegateCommand RemovePersonCommand { get; }
    public DelegateCommand AddPersonCommand { get; }

    public Action<bool?> ExecuteClose { get; set; }

    private bool CanRemovePerson()
    {
        return _currentIndex != -1 && ObservedPeople.Count > _currentIndex;
    }

    private async Task SaveChanges(CancellationToken arg)
    {
        try
        {
            _preferences.ObservedPeople = ObservedPeople.ToArray();
            var response = await _preferencesService.Update(_preferences, arg);

            if (!response)
            {
                _messageBoxService.ShowError("Nenhuma linha foi alterada!");
                return;
            }

            _messageBoxService.ShowInformation("Dados salvos com sucesso!");
            ExecuteClose.Invoke(response);
        }
        catch (Exception e)
        {
            e.Handle(_messageBoxService.ShowError);
        }
    }

    private void RemovePerson()
    {
        ObservedPeople.RemoveAt(_currentIndex);
    }

    private void AddPerson()
    {
        ObservedPeople.Add(_personName);
        PersonName = string.Empty;
    }

    private bool CanAddPerson()
    {
        return !string.IsNullOrEmpty(_personName) && !ObservedPeople.Contains(_personName);
    }

    private async Task OnStartup()
    {
        _preferences = await _preferencesService.GetFirst(CancellationToken.None);
        ObservedPeople.AddRange(_preferences.ObservedPeople);
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        AddPersonCommand.RaiseCanExecuteChanged();
        RemovePersonCommand.RaiseCanExecuteChanged();
        SaveChangesCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }
}
