using BusinessLogic.Interface;
using DTO;
using ManagerWHWpf.Command.Order;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ManagerWHWpf.ViewModels
{
    public class OrdersViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly IOrdersManager _ordersManager;
        private readonly IProductsManager _productsManager;
        private readonly ISuppliersManager _suppliersManager;
        public int CurrentUserId { get; }

        public ObservableCollection<Orders> Orders { get; set; }
        public ObservableCollection<Products> Products { get; set; }
        public ObservableCollection<Suppliers> Suppliers { get; set; }

        private Orders _selectedOrderForEditing;
        public Orders SelectedOrderForEditing
        {
            get => _selectedOrderForEditing;
            set
            {
                _selectedOrderForEditing = value;
                OnPropertyChanged();
                (EditOrderCommand as EditOrderCommand)?.RaiseCanExecuteChanged();
            }
        }

        private Orders _selectedOrderForDeletion;
        public Orders SelectedOrderForDeletion
        {
            get => _selectedOrderForDeletion;
            set
            {
                _selectedOrderForDeletion = value;
                OnPropertyChanged();
                (DeleteOrderCommand as DeleteOrderCommand)?.RaiseCanExecuteChanged();
            }
        }

        private Products _selectedProduct;
        public Products SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                (AddOrderCommand as AddOrderCommand)?.RaiseCanExecuteChanged();
                (EditOrderCommand as EditOrderCommand)?.RaiseCanExecuteChanged();
            }
        }

        private Suppliers _selectedSupplier;
        public Suppliers SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged();
                (AddOrderCommand as AddOrderCommand)?.RaiseCanExecuteChanged();
                (EditOrderCommand as EditOrderCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string _newStatus;
        public string NewStatus
        {
            get => _newStatus;
            set
            {
                _newStatus = value;
                OnPropertyChanged();
                (AddOrderCommand as AddOrderCommand)?.RaiseCanExecuteChanged();
                (EditOrderCommand as EditOrderCommand)?.RaiseCanExecuteChanged();
            }
        }

        private int _newOrderQuantity;
        public int NewOrderQuantity
        {
            get => _newOrderQuantity;
            set
            {
                _newOrderQuantity = value;
                OnPropertyChanged();
                (AddOrderCommand as AddOrderCommand)?.RaiseCanExecuteChanged();
                (EditOrderCommand as EditOrderCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddOrderCommand { get; }
        public ICommand EditOrderCommand { get; }
        public ICommand DeleteOrderCommand { get; }

        public event Action<string> OrderAddFailed;

        public OrdersViewModel(IOrdersManager ordersManager, IProductsManager productsManager, ISuppliersManager suppliersManager, int currentUserId)
        {
            _ordersManager = ordersManager;
            _productsManager = productsManager;
            _suppliersManager = suppliersManager;
            CurrentUserId = currentUserId;

            Orders = new ObservableCollection<Orders>(_ordersManager.GetActiveOrdersByUserId(CurrentUserId));
            Products = new ObservableCollection<Products>(_productsManager.GetAllProducts());
            Suppliers = new ObservableCollection<Suppliers>(_suppliersManager.GetAllSuppliers());

            AddOrderCommand = new AddOrderCommand(_ordersManager, this, HandleOrderAddFailed);
            EditOrderCommand = new EditOrderCommand(_ordersManager, this);
            DeleteOrderCommand = new DeleteOrderCommand(_ordersManager, this);
        }

        public void AddOrder()
        {
            ValidateInputs();

            
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                HandleOrderAddFailed(ErrorMessage);
                return;
            }


            try
            {
                var newOrder = new Orders
                {
                    ProductID = SelectedProduct.ProductID,
                    SupplierID = SelectedSupplier.SupplierID,
                    Status = NewStatus,
                    Quantity = NewOrderQuantity,
                    UserID = CurrentUserId
                };

                _ordersManager.AddOrder(newOrder);
                Orders.Add(newOrder);

                ClearNewOrderFields();
            }
            catch (Exception ex)
            {
                HandleOrderAddFailed($"Failed to add order: {ex.Message}");
            }
        }
        public string ErrorMessage { get; private set; }
        private void ValidateInputs()
        {
            if (SelectedProduct == null)
            {
                SetError("Please select a product.");
                return;
            }
            if (SelectedSupplier == null)
            {
                SetError("Please select a supplier.");
                return;
            }
            if (string.IsNullOrWhiteSpace(NewStatus))
            {
                SetError("Status cannot be empty.");
                return;
            }
            if (NewOrderQuantity <= 0)
            {
                SetError("Quantity must be greater than zero.");
                return;
            }
            ClearError();
        }

        private void SetError(string message)
        {
            ErrorMessage = message;
            MessageBox.Show(ErrorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ClearError()
        {
            ErrorMessage = string.Empty; 
        }

        private void ClearNewOrderFields()
        {
            SelectedProduct = null;
            SelectedSupplier = null;
            NewStatus = string.Empty;
            NewOrderQuantity = 0;
        }

        public void EditSelectedOrder()
        {
            if (SelectedOrderForEditing != null)
            {
                if (!ValidateEditedOrder(out string validationMessage))
                {
                    MessageBox.Show(validationMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SelectedOrderForEditing.ProductID = SelectedProduct?.ProductID ?? SelectedOrderForEditing.ProductID;
                SelectedOrderForEditing.SupplierID = SelectedSupplier?.SupplierID ?? SelectedOrderForEditing.SupplierID;
                SelectedOrderForEditing.Status = NewStatus;
                SelectedOrderForEditing.Quantity = NewOrderQuantity;

                _ordersManager.UpdateOrder(SelectedOrderForEditing);

                var index = Orders.IndexOf(SelectedOrderForEditing);
                if (index >= 0)
                {
                    Orders[index] = SelectedOrderForEditing;
                    OnPropertyChanged(nameof(Orders));
                }

                ClearEditFields();
            }
        }

        private bool ValidateEditedOrder(out string validationMessage)
        {
            if (string.IsNullOrWhiteSpace(NewStatus))
            {
                validationMessage = "Status cannot be empty.";
                return false;
            }
            if (NewOrderQuantity <= 0)
            {
                validationMessage = "Quantity must be greater than zero.";
                return false;
            }
            validationMessage = null;
            return true;
        }

        private void ClearEditFields()
        {
            SelectedOrderForEditing = null;
            SelectedProduct = null;
            SelectedSupplier = null;
            NewStatus = string.Empty;
            NewOrderQuantity = 0;
        }

        public event EventHandler OrderDeleted;

        public void DeleteSelectedOrder()
        {
            if (SelectedOrderForDeletion != null)
            {
                _ordersManager.DeleteOrder(SelectedOrderForDeletion.OrderID);
                Orders.Remove(SelectedOrderForDeletion);

                OrderDeleted?.Invoke(this, EventArgs.Empty);
            }
        }

        public void HandleOrderAddFailed(string errorMessage)
        {
            MessageBox.Show($"Failed to add order: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
