using DTO;

namespace BusinessLogic.Interface
{
    public interface IProductsManager
    {
        Products AddProduct(Products product);
        List<Products> GetAllProducts();
        List<Products> SearchProducts(string searchTerm);
        List<Products> SortProductsByName();
        List<Products> SortProductsByQuantity();
        List<Products> SortProductsByPrice();
        void DeleteProduct(int productId);
        void UpdateProduct(Products product);
    }
}
