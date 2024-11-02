using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManagerWHWpf.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : Window
    {
        public DashboardView()
        {
            InitializeComponent();
        }


        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            
            var productsView = new ProductsView(); 
            productsView.Show(); 
            this.Close(); 
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
           
            var createOrderView = new OrdersView(); 
            createOrderView.Show(); 
            this.Close(); 
        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            
            var activeOrdersView = new SuppliersView(); 
            activeOrdersView.Show(); 
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

