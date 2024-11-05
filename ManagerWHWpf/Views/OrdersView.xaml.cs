using BusinessLogic.Interface;
using DTO;
using ManagerWHWpf.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ManagerWHWpf.Views
{
    public partial class OrdersView : Window
    {
        private readonly IOrdersManager _ordersManager;
        private readonly IProductsManager _productsManager;
        private readonly ISuppliersManager _suppliersManager;
        private readonly int _currentUserId; 

        public OrdersView(IOrdersManager ordersManager, IProductsManager productsManager, ISuppliersManager suppliersManager, int currentUserId)
        {
            InitializeComponent();
            _ordersManager = ordersManager;
            _productsManager = productsManager;
            _suppliersManager = suppliersManager;
            _currentUserId = currentUserId; 

            var viewModel = new OrdersViewModel(ordersManager, productsManager, suppliersManager, currentUserId); // Pass the user ID to the ViewModel
            viewModel.OrderDeleted += (sender, e) =>
            {
                MessageBox.Show("The order has been successfully deleted.");
            };
            viewModel.OrderAddFailed += ViewModel_OrderAddFailed;
            DataContext = viewModel;
        }

        private void ViewModel_OrderDeleted(object sender, EventArgs e)
        {
            MessageBox.Show("The order has been successfully deleted.");
        }

        private void ViewModel_OrderAddFailed(string message)
        {
            MessageBox.Show(message, "Error adding an order", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OrdersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersList.SelectedItem is Orders selectedOrder)
            {
                var viewModel = (OrdersViewModel)DataContext;
                viewModel.SelectedOrderForEditing = selectedOrder; 
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var dashboardView = new DashboardView(_productsManager, _ordersManager, _suppliersManager, _currentUserId);
            dashboardView.Show();
            this.Close();
        }
    }
}
