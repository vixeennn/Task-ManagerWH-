using BusinessLogic.Concrete;
using BusinessLogic.Interface;
using ManagerWHWpf.ViewModels;
using System;
using System.Windows;

namespace ManagerWHWpf.Views
{
    public partial class RegisterView : Window
    {
        private readonly IProductsManager _productsManager;
        private readonly IOrdersManager _ordersManager;
        private readonly ISuppliersManager _suppliersManager;
        public RegisterView(IUsersManager usersManager, IProductsManager productsManager, IOrdersManager ordersManager, ISuppliersManager suppliersManager)
        {
            InitializeComponent();

            var viewModel = new RegisterViewModel(usersManager);
            viewModel.RegisterSuccessful += ViewModel_RegisterSuccessful;
            viewModel.RegisterFailed += ViewModel_RegisterFailed;
            DataContext = viewModel;

            _productsManager = productsManager;
            _ordersManager = ordersManager;
            _suppliersManager = suppliersManager;
        }

        private void ViewModel_RegisterSuccessful(object sender, EventArgs e)
        {
            MessageBox.Show("Registration successful!");
            var loginView = new LoginView(DataContext as IUsersManager, _productsManager, _ordersManager, _suppliersManager);
            loginView.Show();
            this.Close();
        }

        private void ViewModel_RegisterFailed(string message)
        {
            MessageBox.Show(message);
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = (RegisterViewModel)DataContext;
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
