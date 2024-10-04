using DTO;


namespace Dal.Interface
{
    public interface ISuppliersDal
    {
        List<Suppliers> GetAll();
        Suppliers Insert(Suppliers suppliers);
        void Delete(int supplierId);
    }
}
