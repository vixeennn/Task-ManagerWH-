using BusinessLogic.Interface;
using ManagerWHWpf.Command;
using ManagerWHWpf.Command.User;
using ManagerWHWpf.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

public class LoginViewModel : BaseViewModel
{
    private readonly IUsersManager _usersManager;
    private string _username;
    private string _password;

    public event EventHandler LoginSuccessful;

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

    public ICommand LoginCommand { get; }

    public LoginViewModel(IUsersManager usersManager)
    {
        _usersManager = usersManager;
        LoginCommand = new RelayCommand(_ => Login());
    }

    private void Login()
    {
        {

            var user = _usersManager.GetUserByUsernameAndPassword(Username, Password);
            if (user != null)
            {
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }
    }

    public void UpdatePassword(string password)
    {
        Password = password;
    }
}
