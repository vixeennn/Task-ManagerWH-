using BusinessLogic.Interface;
using Dal.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Concrete
{
    public class SuppliersManager : ISuppliersManager
    {
        private readonly ISuppliersDal _suppliersDal;

        public SuppliersManager(ISuppliersDal suppliersDal)
        {
            _suppliersDal = suppliersDal;
        }

        public List<Suppliers> GetAllSuppliers()
        {
            return _suppliersDal.GetAll();
        }

        public Suppliers AddSupplier(Suppliers supplier)
        {
            return _suppliersDal.Insert(supplier);
        }

        public void DeleteSupplier(int supplierId)
        {
            _suppliersDal.Delete(supplierId);
        }
    }
}
