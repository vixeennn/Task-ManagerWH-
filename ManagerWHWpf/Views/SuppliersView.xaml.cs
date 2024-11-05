
    using BusinessLogic.Interface;
    
    using ManagerWHWpf.ViewModels;
    using System;
    using System.Windows;
    using System.Windows.Controls;

    namespace ManagerWHWpf.Views
    {
        public partial class SuppliersView : Window
        {
        private readonly IProductsManager _productsManager;
        private readonly IOrdersManager _ordersManager;
        private readonly ISuppliersManager _suppliersManager;
            private readonly int _currentUserId; 

            public SuppliersView(IProductsManager productsManager, IOrdersManager ordersManager, ISuppliersManager suppliersManager, int currentUserId)
            {
                InitializeComponent();
            _productsManager = productsManager;
            _ordersManager = ordersManager;
            _suppliersManager = suppliersManager;
            _currentUserId = currentUserId; 

                var viewModel = new SuppliersViewModel(suppliersManager);
                viewModel.SupplierDeleted += ViewModel_SupplierDeleted;
                viewModel.SupplierAddFailed += ViewModel_SupplierAddFailed;
                DataContext = viewModel;
            }

            private void ViewModel_SupplierDeleted(object sender, EventArgs e)
            {
                MessageBox.Show("The suppplier has been removed successfully.");
            }

            private void ViewModel_SupplierAddFailed(string message)
            {
                MessageBox.Show(message, "Supplier Addition Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            private void SuppliersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                var viewModel = (SuppliersViewModel)DataContext;
                viewModel.SelectedSupplierForDeletion = (DTO.Suppliers)SuppliersList.SelectedItem;
            }

            private void BackButton_Click(object sender, RoutedEventArgs e)
            {
                var dashboardView = new DashboardView(_productsManager, _ordersManager, _suppliersManager, _currentUserId);
                dashboardView.Show();
                this.Close();
            }
        }
    }


