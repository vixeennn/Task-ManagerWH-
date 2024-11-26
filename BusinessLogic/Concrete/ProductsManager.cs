using BusinessLogic.Interface;
using Dal.Interface;
using DTO;


namespace BusinessLogic.Concrete
{
    public class ProductsManager : IProductsManager
    {
        private readonly IProductsDal _productsDal;

        public ProductsManager(IProductsDal productsDal)
        {
            _productsDal = productsDal;
        }


        public Products AddProduct(Products product)
        {
            return _productsDal.Insert(product);
        }


        public List<Products> GetAllProducts()
        {
            return _productsDal.GetAll();
        }


        public List<Products> SearchProducts(string searchTerm)
        {
            return _productsDal.SearchByNameOrId(searchTerm);
        }


        public List<Products> SortProductsByName()
        {
            return _productsDal.SortByName();
        }


        public List<Products> SortProductsByQuantity()
        {
            return _productsDal.SortByQuantity();
        }


        public List<Products> SortProductsByPrice()
        {
            return _productsDal.SortByPrice();
        }


        public void DeleteProduct(int productId)
        {
            _productsDal.Delete(productId);
        }

        public void UpdateProduct(Products product)
        {
            _productsDal.Update(product);
        }
    }
}
