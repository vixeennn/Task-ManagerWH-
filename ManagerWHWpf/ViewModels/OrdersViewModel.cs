using BusinessLogic.Interface;
using DTO;
using ManagerWHWpf.Command;
using ManagerWHWpf.Command.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerWHWpf.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        public ObservableCollection<Orders> Orders { get; set; }
        public ICommand AddOrderCommand { get; }
        public ICommand EditOrderCommand { get; }
        public ICommand DeleteOrderCommand { get; }
        public ICommand ViewOrdersCommand { get; }

        public OrdersViewModel(IOrdersManager ordersManager)
        {
            Orders = new ObservableCollection<Orders>(ordersManager.GetAllOrders());

            AddOrderCommand = new AddOrderCommand(ordersManager, Orders);
            EditOrderCommand = new EditOrderCommand(ordersManager);
            DeleteOrderCommand = new DeleteOrderCommand(ordersManager, Orders);
            ViewOrdersCommand = new ViewOrderCommand(ordersManager, Orders);
        }
    }
}
