using BusinessLogic.Interface;
using ManagerWHWpf.Command;
using ManagerWHWpf.Command.User;
using ManagerWHWpf.ViewModels;
using System;
using System.Windows.Input;

public class LoginViewModel : BaseViewModel
{
    private readonly IUsersManager _usersManager;
    private string _username;
    private string _password;
    private int _currentUserId; 

    public event EventHandler LoginSuccessful;
    public event Action<string> LoginFailed;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public int CurrentUserId => _currentUserId; 

    public ICommand LoginCommand { get; }

    public LoginViewModel(IUsersManager usersManager)
    {
        _usersManager = usersManager;

        LoginCommand = new LoginCommand(
            usersManager,
            () => Username,  
            () => Password,   
            OnLoginSuccess,
            message => LoginFailed?.Invoke(message));
    }

    private void OnLoginSuccess()
    {
        _currentUserId = _usersManager.GetCurrentUserId(Username); 
        LoginSuccessful?.Invoke(this, EventArgs.Empty);
    }
}
