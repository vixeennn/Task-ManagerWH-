using BusinessLogic.Interface;
using System;
using System.Windows;

namespace ManagerWHWpf.Views
{
    public partial class LoginView : Window
    {
        private readonly IProductsManager _productsManager;
        private readonly IOrdersManager _ordersManager;
        private readonly ISuppliersManager _suppliersManager;
        private int _currentUserId; 

        public LoginView(IUsersManager usersManager, IProductsManager productsManager, IOrdersManager ordersManager, ISuppliersManager suppliersManager)
        {
            InitializeComponent();

            var viewModel = new LoginViewModel(usersManager);
            viewModel.LoginSuccessful += ViewModel_LoginSuccessful;
            viewModel.LoginFailed += ViewModel_LoginFailed;
            DataContext = viewModel;

            _productsManager = productsManager;
            _ordersManager = ordersManager;
            _suppliersManager = suppliersManager;
        }

        private void ViewModel_LoginSuccessful(object sender, EventArgs e)
        {
            _currentUserId = (sender as LoginViewModel)?.CurrentUserId ?? 0; 
            var dashboardView = new DashboardView(_productsManager, _ordersManager, _suppliersManager, _currentUserId);
            dashboardView.Show();
            this.Close();
        }

        private void ViewModel_LoginFailed(string message)
        {
            MessageBox.Show(message);
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = (LoginViewModel)DataContext;
            viewModel.Password = PasswordTextBox.Password;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
