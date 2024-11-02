using ManagerWHWpf.Command;
using ManagerWHWpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ManagerWHWpf.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public ICommand ProductsCommand { get; }
        public ICommand OrdersCommand { get; }
        public ICommand SuppliersCommand { get; }
        public ICommand LogoutCommand { get; }

        public DashboardViewModel()
        {
            ProductsCommand = new RelayCommand(OpenProducts);
            OrdersCommand = new RelayCommand(OpenOrders);
            SuppliersCommand = new RelayCommand(OpenSuppliers);
            LogoutCommand = new RelayCommand(Logout);
        }

        private void OpenProducts(object parameter)
        {
            var productsView = new ProductsView();
            productsView.Show();
        }

        private void OpenOrders(object parameter)
        {
            var ordersView = new OrdersView();
            ordersView.Show();
        }

        private void OpenSuppliers(object parameter)
        {
            var suppliersView = new SuppliersView();
            suppliersView.Show();
        }

        private void Logout(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
