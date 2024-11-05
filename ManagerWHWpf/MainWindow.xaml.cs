using Microsoft.Win32;
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
using ManagerWHWpf.Views;
using BusinessLogic.Concrete;
using Dal.Interface;
using Dal.Concrete;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogic.Interface;

namespace ManagerWHWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private readonly string _connectionString;

        public MainWindow()
        {
            InitializeComponent();

            
            var configuration = LoadConfiguration();
            _connectionString = configuration.GetConnectionString("ManagerWH");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
            var dbConfig = new Configuration(_connectionString); 
            var usersDal = new UsersDal(dbConfig.ConnectionString); 
            var usersManager = new UsersManager(usersDal);
            var productsDal = new ProductsDal(dbConfig.ConnectionString); 
            var productsManager = new ProductsManager(productsDal);
            var ordersDal = new OrdersDal(dbConfig.ConnectionString);
            var ordersManager = new OrdersManager(ordersDal);
            var suppliersDal = new SuppliersDal(dbConfig.ConnectionString);
            var suppliersManager = new SuppliersManager(suppliersDal);


            var loginView = new LoginView(usersManager, productsManager, ordersManager, suppliersManager);
            loginView.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)

        {
            var dbConfig = new Configuration(_connectionString);
            var usersDal = new UsersDal(dbConfig.ConnectionString);
            var usersManager = new UsersManager(usersDal);
            var productsDal = new ProductsDal(dbConfig.ConnectionString);
            var productsManager = new ProductsManager(productsDal);
            var ordersDal = new OrdersDal(dbConfig.ConnectionString);
            var ordersManager = new OrdersManager(ordersDal);
            var suppliersDal = new SuppliersDal(dbConfig.ConnectionString);
            var suppliersManager = new SuppliersManager(suppliersDal);

            var registerView = new RegisterView(usersManager, productsManager, ordersManager, suppliersManager);
            registerView.Show();
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private Microsoft.Extensions.Configuration.IConfiguration LoadConfiguration()
        {
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("manager.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
