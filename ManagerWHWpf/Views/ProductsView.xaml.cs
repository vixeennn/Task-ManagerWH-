using BusinessLogic.Interface;
using ManagerWHWpf.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ManagerWHWpf.Views
{
    public partial class ProductsView : Window
    {
        private readonly IProductsManager _productsManager;
        private readonly IOrdersManager _ordersManager;
        private readonly ISuppliersManager _suppliersManager;

        private readonly int _currentUserId; 

        public ProductsView(IProductsManager productsManager, IOrdersManager ordersManager, ISuppliersManager suppliersManager, int currentUserId)
        {
            InitializeComponent();
            _productsManager = productsManager;
            _ordersManager = ordersManager;
            _suppliersManager = suppliersManager;
            _currentUserId = currentUserId; 

            var viewModel = new ProductsViewModel(productsManager);
            viewModel.ProductDeleted += ViewModel_ProductDeleted;
            viewModel.ProductAddFailed += ViewModel_ProductAddFailed;
            DataContext = viewModel;
        }

        private void ViewModel_ProductDeleted(object sender, EventArgs e)
        {
            MessageBox.Show("The item has been successfully deleted.");
        }

        private void ViewModel_ProductAddFailed(string message)
        {
            MessageBox.Show(message, "Error adding a product", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (ProductsViewModel)DataContext;
            viewModel.SelectedProductForDeletion = (DTO.Products)ProductsList.SelectedItem;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var dashboardView = new DashboardView(_productsManager, _ordersManager, _suppliersManager, _currentUserId);
            dashboardView.Show();
            this.Close();
        }
    }
}