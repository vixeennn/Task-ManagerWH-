using BusinessLogic.Interface;
using ManagerWHWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerWHWpf.Command.Product
{
    public class DeleteProductCommand : ICommand
    {
        private readonly IProductsManager _productsManager;
        private readonly ProductsViewModel _viewModel;

        public DeleteProductCommand(IProductsManager productsManager, ProductsViewModel viewModel)
        {
            _productsManager = productsManager;
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _viewModel.SelectedProductForDeletion != null;

        public void Execute(object parameter)
        {
            if (_viewModel.SelectedProductForDeletion != null)
            {
                
                _productsManager.DeleteProduct(_viewModel.SelectedProductForDeletion.ProductID);
              
                _viewModel.RefreshProductList();
               
                _viewModel.SelectedProductForDeletion = null; 
            }
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

}
