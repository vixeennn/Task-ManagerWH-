using System;
using System.Windows.Input;
using BusinessLogic.Interface;
using ManagerWHWpf.ViewModels;

namespace ManagerWHWpf.Command.Order
{
    public class DeleteOrderCommand : ICommand
    {
        private readonly IOrdersManager _ordersManager;
        private readonly OrdersViewModel _viewModel;

        public DeleteOrderCommand(IOrdersManager ordersManager, OrdersViewModel viewModel)
        {
            _ordersManager = ordersManager;
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _viewModel.SelectedOrderForEditing != null; 
        }

        public void Execute(object parameter)
        {
            _viewModel.DeleteSelectedOrder();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
