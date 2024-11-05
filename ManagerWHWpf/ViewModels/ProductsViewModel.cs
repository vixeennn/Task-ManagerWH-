using BusinessLogic.Interface;
using DTO;
using ManagerWHWpf.Command.Product;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

namespace ManagerWHWpf.ViewModels
{
    public class ProductsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly IProductsManager _productsManager;

        public ObservableCollection<string> SortOptions { get; set; } = new ObservableCollection<string> { "Name", "Quantity", "Price" };

        private ObservableCollection<Products> _products;
        public ObservableCollection<Products> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
                UpdateFilteredProducts();
            }
        }

        private ObservableCollection<Products> _filteredProducts;
        public ObservableCollection<Products> FilteredProducts
        {
            get => _filteredProducts;
            set
            {
                _filteredProducts = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                UpdateFilteredProducts();
            }
        }

        private string _newProductName;
        public string NewProductName
        {
            get => _newProductName;
            set
            {
                _newProductName = value;
                OnPropertyChanged();
                (AddProductCommand as AddProductCommand)?.RaiseCanExecuteChanged();
            }
        }

        private int _newProductQuantity;
        public int NewProductQuantity
        {
            get => _newProductQuantity;
            set
            {
                _newProductQuantity = value;
                OnPropertyChanged();
                (AddProductCommand as AddProductCommand)?.RaiseCanExecuteChanged();
            }
        }

        private decimal _newProductPrice;
        public decimal NewProductPrice
        {
            get => _newProductPrice;
            set
            {
                _newProductPrice = value;
                OnPropertyChanged();
                (AddProductCommand as AddProductCommand)?.RaiseCanExecuteChanged();
            }
        }

        private Products _selectedProductForDeletion;
        public Products SelectedProductForDeletion
        {
            get => _selectedProductForDeletion;
            set
            {
                _selectedProductForDeletion = value;
                OnPropertyChanged();
                (DeleteProductCommand as DeleteProductCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                _selectedSortOption = value;
                OnPropertyChanged();
                SortProducts();
            }
        }

        public ICommand AddProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public event EventHandler ProductDeleted;
        public event Action<string> ProductAddFailed;

        public ProductsViewModel(IProductsManager productsManager)
        {
            _productsManager = productsManager;
            AddProductCommand = new AddProductCommand(productsManager, this, HandleProductAddFailed);
            DeleteProductCommand = new DeleteProductCommand(productsManager, this);

            RefreshProductList();
        }

        public void RefreshProductList()
        {
            Products = new ObservableCollection<Products>(_productsManager.GetAllProducts());
            UpdateFilteredProducts();
        }

        public void UpdateFilteredProducts()
        {
            FilteredProducts = new ObservableCollection<Products>(
                string.IsNullOrWhiteSpace(SearchText)
                    ? Products
                    : Products.Where(p => p.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        private void SortProducts()
        {
            if (_selectedSortOption == "Name")
                FilteredProducts = new ObservableCollection<Products>(FilteredProducts.OrderBy(p => p.Name));
            else if (_selectedSortOption == "Quantity")
                FilteredProducts = new ObservableCollection<Products>(FilteredProducts.OrderBy(p => p.QuantityInStock));
            else if (_selectedSortOption == "Price")
                FilteredProducts = new ObservableCollection<Products>(FilteredProducts.OrderBy(p => p.Price));
        }

        public void DeleteSelectedProduct()
        {
            if (SelectedProductForDeletion != null)
            {
                _productsManager.DeleteProduct(SelectedProductForDeletion.ProductID);
                Products.Remove(SelectedProductForDeletion);
                ProductDeleted?.Invoke(this, EventArgs.Empty);
                UpdateFilteredProducts();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void HandleProductAddFailed(string errorMessage)
        {
            MessageBox.Show($"Failed to add product: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
