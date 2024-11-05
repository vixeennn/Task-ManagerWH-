using BusinessLogic.Interface;
using DTO;
using ManagerWHWpf.Command.Supplier;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ManagerWHWpf.ViewModels
{
    public class SuppliersViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly ISuppliersManager _suppliersManager;

        public ObservableCollection<Suppliers> Suppliers { get; private set; } = new ObservableCollection<Suppliers>();

        private Suppliers _selectedSupplierForDeletion;
        public Suppliers SelectedSupplierForDeletion
        {
            get => _selectedSupplierForDeletion;
            set
            {
                if (_selectedSupplierForDeletion != value)
                {
                    _selectedSupplierForDeletion = value;
                    OnPropertyChanged();
                    (DeleteSupplierCommand as DeleteSupplierCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _newSupplierName;
        public string NewSupplierName
        {
            get => _newSupplierName;
            set
            {
                if (_newSupplierName != value)
                {
                    _newSupplierName = value;
                    OnPropertyChanged();
                    (AddSupplierCommand as AddSupplierCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _newSupplierPhone;
        public string NewSupplierPhone
        {
            get => _newSupplierPhone;
            set
            {
                if (_newSupplierPhone != value)
                {
                    _newSupplierPhone = value;
                    OnPropertyChanged();
                    (AddSupplierCommand as AddSupplierCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _newSupplierAddress;
        public string NewSupplierAddress
        {
            get => _newSupplierAddress;
            set
            {
                if (_newSupplierAddress != value)
                {
                    _newSupplierAddress = value;
                    OnPropertyChanged();
                    (AddSupplierCommand as AddSupplierCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand AddSupplierCommand { get; }
        public ICommand DeleteSupplierCommand { get; }

        public event EventHandler SupplierDeleted;
       
        public event Action<string> SupplierAddFailed;

        public SuppliersViewModel(ISuppliersManager suppliersManager)
        {
            _suppliersManager = suppliersManager;
            
            AddSupplierCommand = new AddSupplierCommand(suppliersManager, this, message => SupplierAddFailed?.Invoke(message));
            DeleteSupplierCommand = new DeleteSupplierCommand(suppliersManager, this);

            RefreshSupplierList(); 
        }
        public void RefreshSupplierList()
        {
            Suppliers.Clear();
            var suppliersList = _suppliersManager.GetAllSuppliers();
            foreach (var supplier in suppliersList)
            {
                Suppliers.Add(supplier);
            }
        }

        public void DeleteSelectedSupplier()
        {
            if (SelectedSupplierForDeletion != null)
            {
                _suppliersManager.DeleteSupplier(SelectedSupplierForDeletion.SupplierID);
                Suppliers.Remove(SelectedSupplierForDeletion);
                SupplierDeleted?.Invoke(this, EventArgs.Empty);
                RefreshSupplierList(); 
            }
        }

        public void AddSupplier()
        {
            var newSupplier = new Suppliers
            {
                Name = NewSupplierName,
                Phone = NewSupplierPhone,
                Address = NewSupplierAddress
            };

            try
            {
                _suppliersManager.AddSupplier(newSupplier);
                Suppliers.Add(newSupplier);
                
                NewSupplierName = string.Empty;
                NewSupplierPhone = string.Empty;
                NewSupplierAddress = string.Empty;
            }
            catch (Exception ex)
            {
                SupplierAddFailed?.Invoke(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
