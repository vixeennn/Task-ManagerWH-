using ManagerWHWpf.Command;
using ManagerWHWpf.Views;
using System.Windows.Input;
using System.Windows;
using BusinessLogic.Interface;
using ManagerWHWpf.Command.User;
using BusinessLogic.Concrete;

namespace ManagerWHWpf.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public ICommand ProductsCommand { get; }
        public ICommand OrdersCommand { get; }
        public ICommand SuppliersCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand DeleteUserCommand { get; }

        private readonly IUsersManager _usersManager;
        private readonly IProductsManager _productsManager;
        private readonly IOrdersManager _ordersManager;
        private readonly ISuppliersManager _suppliersManager;
        private readonly int _currentUserId; 

        public DashboardViewModel(IUsersManager usersManager, IProductsManager productsManager, IOrdersManager ordersManager, ISuppliersManager suppliersManager, int currentUserId)
        {
            _usersManager = usersManager;
            _productsManager = productsManager;
            _ordersManager = ordersManager;
            _suppliersManager = suppliersManager;
            _currentUserId = currentUserId; 

            ProductsCommand = new RelayCommand(OpenProducts);
            OrdersCommand = new RelayCommand(OpenOrders);
            SuppliersCommand = new RelayCommand(OpenSuppliers);
            LogoutCommand = new RelayCommand(Logout);

           
        }

        private void OpenProducts(object parameter)
        {
           
            var productsView = new ProductsView(_productsManager, _ordersManager, _suppliersManager, _currentUserId);
            productsView.Show();
        }

        private void OpenOrders(object parameter)
        {
            
            var ordersView = new OrdersView(_ordersManager, _productsManager, _suppliersManager, _currentUserId);
            ordersView.Show();
        }

        private void OpenSuppliers(object parameter)
        {
            var suppliersView = new SuppliersView(_productsManager, _ordersManager, _suppliersManager,  _currentUserId);
            suppliersView.Show();
        }

        private void Logout(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
