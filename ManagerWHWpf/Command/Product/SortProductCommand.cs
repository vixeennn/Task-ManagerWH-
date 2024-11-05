using BusinessLogic.Interface;
using ManagerWHWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerWHWpf.Command.Product
{
    public class SortProductsCommand : ICommand
    {
        private readonly IProductsManager _productsManager;
        private readonly ProductsViewModel _viewModel;

        public SortProductsCommand(IProductsManager productsManager, ProductsViewModel viewModel)
        {
            _productsManager = productsManager;
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        // Повертає true, якщо SelectedSortOption не пустий
        public bool CanExecute(object parameter) => !string.IsNullOrEmpty(_viewModel.SelectedSortOption);

        public void Execute(object parameter)
        {
            List<DTO.Products> sortedProducts;

            switch (_viewModel.SelectedSortOption)
            {
                case "Name":
                    sortedProducts = _productsManager.SortProductsByName();
                    break;
                case "Quantity in Stock":
                    sortedProducts = _productsManager.SortProductsByQuantity();
                    break;
                case "Price":
                    sortedProducts = _productsManager.SortProductsByPrice();
                    break;
                default:
                    return;
            }

           
            _viewModel.Products = new ObservableCollection<DTO.Products>(sortedProducts);
        }

       
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
