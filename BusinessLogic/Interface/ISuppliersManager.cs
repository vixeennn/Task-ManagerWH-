using DTO;

namespace BusinessLogic.Interface
{
    public interface ISuppliersManager
    {
        List<Suppliers> GetAllSuppliers();
        Suppliers AddSupplier(Suppliers supplier);
        void DeleteSupplier(int supplierId);
    }
}
