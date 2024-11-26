using Dal.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dal.Concrete
{
    public class ProductsDal : IProductsDal
    {
        private readonly SqlConnection _connection;

        public ProductsDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        
        public List<Products> GetAll()
        {
            return ExecuteProductQuery("SELECT ProductID, Name, QuantityInStock, Price FROM Products");
        }

        public Products Insert(Products products)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Products (Name, QuantityInStock, Price) OUTPUT inserted.ProductID VALUES (@Name, @QuantityInStock, @Price)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("Name", products.Name);
                command.Parameters.AddWithValue("QuantityInStock", products.QuantityInStock);
                command.Parameters.AddWithValue("Price", products.Price);

                _connection.Open();
                products.ProductID = Convert.ToInt32(command.ExecuteScalar());
                _connection.Close();
                return products;
            }
        }

    
        public List<Products> SearchByNameOrId(string searchTerm)
        {
            var query = @"
            SELECT ProductID, Name, QuantityInStock, Price 
            FROM Products 
            WHERE Name LIKE @SearchTerm OR ProductID = @ProductId";

            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                int productId;
                if (int.TryParse(searchTerm, out productId))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                }
                else
                {
                    command.Parameters.AddWithValue("@ProductId", DBNull.Value);
                }

                return ExecuteProductQuery(command);
            }
        }

        
        public List<Products> SortByName()
        {
            return ExecuteProductQuery("SELECT ProductID, Name, QuantityInStock, Price FROM Products ORDER BY Name");
        }

        
        public List<Products> SortByQuantity()
        {
            return ExecuteProductQuery("SELECT ProductID, Name, QuantityInStock, Price FROM Products ORDER BY QuantityInStock DESC");
        }

   
        public List<Products> SortByPrice()
        {
            return ExecuteProductQuery("SELECT ProductID, Name, QuantityInStock, Price FROM Products ORDER BY Price");
        }

        
        private List<Products> ExecuteProductQuery(string query)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = query;
                return ExecuteProductQuery(command);
            }
        }

       
        private List<Products> ExecuteProductQuery(SqlCommand command)
        {
            var products = new List<Products>();

            _connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Products
                {
                    ProductID = Convert.ToInt32(reader["ProductID"]),
                    Name = reader["Name"].ToString(),
                    QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                    Price = Convert.ToDecimal(reader["Price"])
                });
            }

            _connection.Close();
            return products;
        }

        public void Delete(int productId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Products WHERE ProductID = @ProductID";
                command.Parameters.AddWithValue("@ProductID", productId);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Update(Products product)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Products SET Name = @Name, QuantityInStock = @QuantityInStock, Price = @Price WHERE ProductID = @ProductID";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                command.Parameters.AddWithValue("@Price", product.Price);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}
