
using DTO;

namespace Dal.Interface
{
    public interface IProductsDal
    {
        List<Products> GetAll();
        Products Insert(Products products);
        List<Products> SearchByNameOrId(string searchTerm);
        List<Products> SortByName();
        List<Products> SortByQuantity();
        List<Products> SortByPrice();
        void Delete(int productId);
    }
}
