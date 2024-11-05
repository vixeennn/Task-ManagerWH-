using BusinessLogic.Interface;
using DTO;
using ManagerWHWpf.ViewModels;
using System;
using System.Windows.Input;

namespace ManagerWHWpf.Command.Product
{
    public class AddProductCommand : ICommand
    {
        private readonly IProductsManager _productsManager;
        private readonly ProductsViewModel _viewModel;
        private readonly Action<string> _handleAddFailed;

        public AddProductCommand(IProductsManager productsManager, ProductsViewModel viewModel, Action<string> handleAddFailed)
        {
            _productsManager = productsManager;
            _viewModel = viewModel;
            _handleAddFailed = handleAddFailed;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.NewProductName) &&
                   _viewModel.NewProductQuantity > 0 &&
                   _viewModel.NewProductPrice > 0;
        }

        public void Execute(object parameter)
        {
            try
            {
                var newProduct = new Products
                {
                    Name = _viewModel.NewProductName,
                    QuantityInStock = _viewModel.NewProductQuantity,
                    Price = _viewModel.NewProductPrice
                };

                _productsManager.AddProduct(newProduct);
                _viewModel.Products.Add(newProduct);

              
                _viewModel.NewProductName = string.Empty;
                _viewModel.NewProductQuantity = 0;
                _viewModel.NewProductPrice = 0;

                _viewModel.UpdateFilteredProducts();
            }
            catch (Exception ex)
            {
                _handleAddFailed?.Invoke(ex.Message);
            }
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
