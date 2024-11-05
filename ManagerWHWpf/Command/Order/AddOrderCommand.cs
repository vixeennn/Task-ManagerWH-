using BusinessLogic.Interface;
using ManagerWHWpf.ViewModels;
using System.Windows.Input;
using System;

public class AddOrderCommand : ICommand
{
    private readonly IOrdersManager _ordersManager;
    private readonly OrdersViewModel _viewModel;

    public event EventHandler CanExecuteChanged;

    public AddOrderCommand(IOrdersManager ordersManager, OrdersViewModel viewModel, Action<string> handleOrderAddFailed)
    {
        _ordersManager = ordersManager;
        _viewModel = viewModel;
    }

    public bool CanExecute(object parameter)
    {
        
        return _viewModel.SelectedProduct != null &&
               _viewModel.SelectedSupplier != null &&
               !string.IsNullOrWhiteSpace(_viewModel.NewStatus) &&
               _viewModel.NewOrderQuantity > 0;
    }

    public void Execute(object parameter)
    {
        _viewModel.AddOrder();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
