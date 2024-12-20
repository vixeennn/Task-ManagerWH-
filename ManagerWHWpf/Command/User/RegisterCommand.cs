﻿using BusinessLogic.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerWHWpf.Command.User
{
    public class RegisterCommand : ICommand
    {
        private readonly IUsersManager _usersManager;
        private readonly Func<string> _getUsername;
        private readonly Func<string> _getPassword;
        private readonly Action _onRegisterSuccess;
        private readonly Action<string> _onRegisterFailure;

        public RegisterCommand(
            IUsersManager usersManager,
            Func<string> getUsername,
            Func<string> getPassword,
            Action onRegisterSuccess,
            Action<string> onRegisterFailure)
        {
            _usersManager = usersManager;
            _getUsername = getUsername;
            _getPassword = getPassword;
            _onRegisterSuccess = onRegisterSuccess;
            _onRegisterFailure = onRegisterFailure;
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

            if (_usersManager.GetUserByUsername(username) == null)
            {
                var newUser = new Users
                {
                    Username = username,
                    Password = password 
                };

                _usersManager.AddUser(newUser);
                _onRegisterSuccess?.Invoke();
            }
            else
            {
                _onRegisterFailure?.Invoke("Username is already taken.");
            }
        }
    }
}
