using BusinessLogic.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerWHWpf.Command.Order
{
    public class DeleteOrderCommand : ICommand
    {
        private readonly IOrdersManager _ordersManager;
        private readonly ObservableCollection<Orders> _ordersCollection;

        public DeleteOrderCommand(IOrdersManager ordersManager, ObservableCollection<Orders> ordersCollection)
        {
            _ordersManager = ordersManager;
            _ordersCollection = ordersCollection;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => parameter is Orders;

        public void Execute(object parameter)
        {
            if (parameter is Orders order)
            {
                _ordersManager.DeleteOrder(order.OrderID);
                _ordersCollection.Remove(order);
            }
        }
    }
}
