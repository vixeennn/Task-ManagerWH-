using BusinessLogic.Interface;
using System;
using System.Windows;

namespace ManagerWHWpf.Views
{
    public partial class LoginView : Window
    {
        private readonly IUsersManager _usersManager;

        public LoginView(IUsersManager usersManager)
        {
            InitializeComponent();
            _usersManager = usersManager;

            var viewModel = new LoginViewModel(usersManager);
            viewModel.LoginSuccessful += ViewModel_LoginSuccessful;
            DataContext = viewModel;
        }

        private void ViewModel_LoginSuccessful(object? sender, EventArgs e)
        {
            var dashboardView = new DashboardView();
            dashboardView.Show();
            this.Close();
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UsernamePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                UsernamePlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                PasswordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = (LoginViewModel)DataContext;
            viewModel.UpdatePassword(PasswordTextBox.Password);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
