using BusinessLogic.Interface;
using DTO;
using ManagerWHWpf.ViewModels;
using System;
using System.Windows.Input;

namespace ManagerWHWpf.Command.Supplier
{
    public class AddSupplierCommand : ICommand
    {
        private readonly ISuppliersManager _suppliersManager;
        private readonly SuppliersViewModel _viewModel;
        private readonly Action<string> _handleAddFailed; 

        public AddSupplierCommand(ISuppliersManager suppliersManager, SuppliersViewModel viewModel, Action<string> handleAddFailed)
        {
            _suppliersManager = suppliersManager;
            _viewModel = viewModel;
            _handleAddFailed = handleAddFailed; 
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.NewSupplierName) &&
                   !string.IsNullOrEmpty(_viewModel.NewSupplierPhone) &&
                   !string.IsNullOrEmpty(_viewModel.NewSupplierAddress);
        }

        public void Execute(object parameter)
        {
            try
            {
                var newSupplier = new Suppliers
                {
                    Name = _viewModel.NewSupplierName,
                    Phone = _viewModel.NewSupplierPhone,
                    Address = _viewModel.NewSupplierAddress
                };

                _suppliersManager.AddSupplier(newSupplier);
                _viewModel.Suppliers.Add(newSupplier);

                
                _viewModel.NewSupplierName = string.Empty;
                _viewModel.NewSupplierPhone = string.Empty;
                _viewModel.NewSupplierAddress = string.Empty;
            }
            catch (Exception ex)
            {
                _handleAddFailed?.Invoke(ex.Message); 
            }
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
