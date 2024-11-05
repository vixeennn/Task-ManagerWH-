using BusinessLogic.Interface;
using ManagerWHWpf.ViewModels;
using System;
using System.Windows.Input;

namespace ManagerWHWpf.Command.Supplier
{
    public class DeleteSupplierCommand : ICommand
    {
        private readonly ISuppliersManager _suppliersManager;
        private readonly SuppliersViewModel _viewModel;

        public DeleteSupplierCommand(ISuppliersManager suppliersManager, SuppliersViewModel viewModel)
        {
            _suppliersManager = suppliersManager;
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _viewModel.SelectedSupplierForDeletion != null;

        public void Execute(object parameter)
        {
            if (_viewModel.SelectedSupplierForDeletion != null)
            {
                
                _suppliersManager.DeleteSupplier(_viewModel.SelectedSupplierForDeletion.SupplierID);
                _viewModel.RefreshSupplierList();
                _viewModel.SelectedSupplierForDeletion = null; 
            }
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
