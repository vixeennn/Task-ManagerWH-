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
   
        public class AddOrderCommand : ICommand
        {
            private readonly IOrdersManager _ordersManager;
            private readonly ObservableCollection<Orders> _ordersCollection;

            public AddOrderCommand(IOrdersManager ordersManager, ObservableCollection<Orders> ordersCollection)
            {
                _ordersManager = ordersManager;
                _ordersCollection = ordersCollection;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                var newOrder = parameter as Orders ?? new Orders { OrderDate = DateTime.Now, Status = "New" };
                _ordersManager.AddOrder(newOrder);
                _ordersCollection.Add(newOrder);
            }
        }
    
}
