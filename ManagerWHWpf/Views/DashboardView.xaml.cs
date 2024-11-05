using BusinessLogic.Interface;
using System.Windows;

namespace ManagerWHWpf.Views
{
    public partial class DashboardView : Window
    {
        private readonly IProductsManager _productsManager;
        private readonly IOrdersManager _ordersManager;
        private readonly ISuppliersManager _suppliersManager;
        private readonly int _currentUserId; 

        public DashboardView(IProductsManager productsManager, IOrdersManager ordersManager, ISuppliersManager suppliersManager, int currentUserId)
        {
            InitializeComponent();
            _productsManager = productsManager;
            _ordersManager = ordersManager;
            _suppliersManager = suppliersManager;
            _currentUserId = currentUserId; 
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            
            var productsView = new ProductsView(_productsManager, _ordersManager, _suppliersManager, _currentUserId);
            productsView.Show();
            this.Close();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
          
            var createOrderView = new OrdersView(_ordersManager, _productsManager, _suppliersManager, _currentUserId);
            createOrderView.Show();
            this.Close();
        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            var suppliersView = new SuppliersView(_productsManager, _ordersManager, _suppliersManager, _currentUserId);
            suppliersView.Show();
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
