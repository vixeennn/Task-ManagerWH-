using BusinessLogic.Interface;
using ManagerWHWpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using DTO;
using ManagerWHWpf.Command.User;

namespace ManagerWHWpf.ViewModels
{

    public class RegisterViewModel : BaseViewModel
    {
        private readonly IUsersManager _usersManager;
        private string _username;
        private string _password;

        public event EventHandler RegisterSuccessful;
        public event Action<string> RegisterFailed;

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

        public ICommand RegisterCommand { get; }

        public RegisterViewModel(IUsersManager usersManager)
        {
            _usersManager = usersManager;

            RegisterCommand = new RegisterCommand(
                usersManager,
                () => Username,
                () => Password,
                OnRegisterSuccess,
                message => RegisterFailed?.Invoke(message));
        }

        private void OnRegisterSuccess()
        {
            RegisterSuccessful?.Invoke(this, EventArgs.Empty);
        }
    }


}
