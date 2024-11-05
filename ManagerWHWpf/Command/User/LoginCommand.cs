using BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerWHWpf.Command.User
{
    public class LoginCommand : ICommand
    {
        private readonly IUsersManager _usersManager;
        private readonly Action _onLoginSuccess;
        private readonly Action<string> _onLoginFailure;
        private readonly Func<string> _getUsername;
        private readonly Func<string> _getPassword;

        public LoginCommand(IUsersManager usersManager, Func<string> getUsername, Func<string> getPassword, Action onLoginSuccess, Action<string> onLoginFailure)
        {
            _usersManager = usersManager;
            _getUsername = getUsername;
            _getPassword = getPassword;
            _onLoginSuccess = onLoginSuccess;
            _onLoginFailure = onLoginFailure;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var username = _getUsername();
            var password = _getPassword();

            var user = _usersManager.GetUserByUsernameAndPassword(username, password);
            if (user != null)
            {
                _onLoginSuccess?.Invoke();
            }
            else
            {
                _onLoginFailure?.Invoke("Incorrect username or password. Try again.");
            }
        }
    }

}

