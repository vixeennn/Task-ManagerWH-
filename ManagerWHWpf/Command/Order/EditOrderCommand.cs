using BusinessLogic.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerWHWpf.Command.Order
{
    public class EditOrderCommand : ICommand
    {
        private readonly IOrdersManager _ordersManager;

        public EditOrderCommand(IOrdersManager ordersManager)
        {
            _ordersManager = ordersManager;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => parameter is Orders;

        public void Execute(object parameter)
        {
            if (parameter is Orders order)
            {
                order.Status = "Updated"; // Змінюємо статус для прикладу
                _ordersManager.UpdateOrder(order);
            }
        }
    }
}
