using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BusinessLogic.Interface;
using ManagerWHWpf.ViewModels;

namespace ManagerWHWpf.Command.Order
{
    public class EditOrderCommand : ICommand
    {
        private readonly IOrdersManager _ordersManager;
        private readonly OrdersViewModel _viewModel;

        public EditOrderCommand(IOrdersManager ordersManager, OrdersViewModel viewModel)
        {
            _ordersManager = ordersManager;
            _viewModel = viewModel;

           
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.SelectedOrderForEditing) ||
                    e.PropertyName == nameof(_viewModel.SelectedProduct) ||
                    e.PropertyName == nameof(_viewModel.SelectedSupplier) ||
                    e.PropertyName == nameof(_viewModel.NewStatus) ||
                    e.PropertyName == nameof(_viewModel.NewOrderQuantity))
                {
                    RaiseCanExecuteChanged();
                }
            };
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) =>
            _viewModel.SelectedOrderForEditing != null &&
            _viewModel.SelectedProduct != null &&
            _viewModel.SelectedSupplier != null &&
            !string.IsNullOrWhiteSpace(_viewModel.NewStatus) &&
            _viewModel.NewOrderQuantity > 0;

        public void Execute(object parameter)
        {
            _viewModel.EditSelectedOrder();
            
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
