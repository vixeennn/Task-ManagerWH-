using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using BusinessLogic.Concrete;
using Dal.Interface;
using DTO;

namespace BLTests
{
    [TestFixture]
    public class ProductsManagerTest
    {
        private ProductsManager _productsManager;
        private Mock<IProductsDal> _mockProductsDal;

        [SetUp]
        public void Setup()
        {
            _mockProductsDal = new Mock<IProductsDal>();
            _productsManager = new ProductsManager(_mockProductsDal.Object);
        }

        [Test]
        //public void AddProduct_ShouldReturnAddedProduct()
        //{
        //    // Arrange
        //    var product = new Products { ProductID = 1, Name = "Test Product" };
        //    _mockProductsDal.Setup(dal => dal.Insert(It.IsAny<Products>())).Returns(product);

        //    // Act
        //    var result = _productsManager.AddProduct(product);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.ProductID);
        //    Assert.AreEqual("Test Product", result.Name);
        //}
        public void AddProduct_ShouldReturnAddedProduct_WithGeneratedID()
        {
            // Arrange
            var productToAdd = new Products { Name = "Test Product" }; 
            var expectedProduct = new Products { ProductID = 1, Name = "Test Product" }; 

            
            _mockProductsDal.Setup(dal => dal.Insert(It.IsAny<Products>()))
                .Returns((Products product) =>
                {
                    
                    product.ProductID = 1;
                    return product;
                });

            // Act
            var result = _productsManager.AddProduct(productToAdd);

            // Assert
            Assert.IsNotNull(result); 
            Assert.AreEqual(1, result.ProductID); 
            Assert.AreEqual("Test Product", result.Name);
        }

        [Test]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var productsList = new List<Products> { new Products { ProductID = 1, Name = "Product1" } };
            _mockProductsDal.Setup(dal => dal.GetAll()).Returns(productsList);

            // Act
            var result = _productsManager.GetAllProducts();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Product1", result[0].Name);
        }

        [Test]
        public void SearchProducts_ShouldReturnMatchingProducts()
        {
            // Arrange
            string searchTerm = "Test";
            var productsList = new List<Products> { new Products { ProductID = 2, Name = "Test Product" } };
            _mockProductsDal.Setup(dal => dal.SearchByNameOrId(searchTerm)).Returns(productsList);

            // Act
            var result = _productsManager.SearchProducts(searchTerm);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Test Product", result[0].Name);
        }

        [Test]
        public void SortProductsByName_ShouldReturnProductsSortedByName()
        {
            // Arrange
            var sortedProducts = new List<Products>
            {
                new Products { ProductID = 1, Name = "A Product" },
                new Products { ProductID = 2, Name = "B Product" }
            };
            _mockProductsDal.Setup(dal => dal.SortByName()).Returns(sortedProducts);

            // Act
            var result = _productsManager.SortProductsByName();

            // Assert
            Assert.AreEqual("A Product", result[0].Name);
            Assert.AreEqual("B Product", result[1].Name);
        }

       

        [Test]
        public void SortProductsByPrice_ShouldReturnProductsSortedByPrice()
        {
            // Arrange
            var sortedProducts = new List<Products>
            {
                new Products { ProductID = 1, Price = 5.99m },
                new Products { ProductID = 2, Price = 10.99m }
            };
            _mockProductsDal.Setup(dal => dal.SortByPrice()).Returns(sortedProducts);

            // Act
            var result = _productsManager.SortProductsByPrice();

            // Assert
            Assert.AreEqual(5.99m, result[0].Price);
            Assert.AreEqual(10.99m, result[1].Price);
        }

        [Test]
        public void DeleteProduct_ShouldInvokeDeleteOnDal()
        {
            // Arrange
            int productId = 3;

            // Act
            _productsManager.DeleteProduct(productId);

            // Assert
            _mockProductsDal.Verify(dal => dal.Delete(productId), Times.Once);
        }
    }
}
