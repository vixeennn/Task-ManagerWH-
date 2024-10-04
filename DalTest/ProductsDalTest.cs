using Dal.Concrete;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DalTest
{
   

    
        public class ProductsDalTests
        {
            private readonly ProductsDal _dal;
            private List<Products> _productsInDb = new List<Products>();

            public ProductsDalTests()
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("manager.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("ManagerWH") ?? "";
                _dal = new ProductsDal(connectionString);
            }

            [SetUp]
            public void Setup()
            {
                var product1 = new Products { Name = "Product 1", QuantityInStock = 10, Price = 100.00m };
                _productsInDb.Add(_dal.Insert(product1));

                var product2 = new Products { Name = "Product 2", QuantityInStock = 20, Price = 150.50m };
                _productsInDb.Add(_dal.Insert(product2));

                var product3 = new Products { Name = "Product 3", QuantityInStock = 30, Price = 200.00m };
                _productsInDb.Add(_dal.Insert(product3));
            }

            [TearDown]
            public void TearDown()
            {
                foreach (var product in _productsInDb)
                {
                    _dal.Delete(product.ProductID);
                }
                _productsInDb.Clear();
            }

           
       

        [Test]
            public void InsertTest()
            {
                var newProduct = new Products { Name = "Product 4", QuantityInStock = 15, Price = 120.00m };
                var insertedProduct = _dal.Insert(newProduct);

                if (insertedProduct.ProductID == 0)
                {
                    throw new Exception("InsertTest failed: Product ID should not be zero.");
                }

                _dal.Delete(insertedProduct.ProductID);
            }

             [Test]
            public void SearchByNameOrIdTest()
            {
                var searchResults = _dal.SearchByNameOrId("Product 1");

                if (searchResults.Count != 1 || searchResults[0].Name != "Product 1")
                {
                    throw new Exception("SearchByNameOrIdTest failed: Expected to find 'Product 1'.");
                }

                searchResults = _dal.SearchByNameOrId(_productsInDb.First().ProductID.ToString());

                if (searchResults.Count != 1 || searchResults[0].ProductID != _productsInDb.First().ProductID)
                {
                    throw new Exception("SearchByNameOrIdTest failed: Expected to find the product by ID.");
                }
            }

            
        
        
        


        [Test]
            public void DeleteTest()
            {
                var productToDelete = _productsInDb.First();
                _dal.Delete(productToDelete.ProductID);
                var deletedProduct = _dal.GetAll().FirstOrDefault(p => p.ProductID == productToDelete.ProductID);

                if (deletedProduct != null)
                {
                    throw new Exception("DeleteTest failed: Product still exists after deletion.");
                }
            }
        }
    

}
