using BusinessLogic.Concrete;
using BusinessLogic.Interface;
using Dal.Concrete;
using Dal.Interface;
using ManagerWHWpf.ViewModels;
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
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : Window
    {
        public OrdersView()
        {
            InitializeComponent();
            //IOrdersDal ordersDal = new OrdersDal(); // Ваша реалізація IOrdersDal
            //IOrdersManager ordersManager = new OrdersManager(ordersDal); // Створення OrdersManager
            //DataContext = new OrdersViewModel(ordersManager);
        }
    }
}
