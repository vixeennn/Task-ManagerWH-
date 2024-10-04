using Dal.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dal.Concrete
{
    public class OrdersDal : IOrdersDal
    {
        private readonly string _connectionString;

        public OrdersDal(string connectionString)
        {
            _connectionString = connectionString;
        }

       
        public List<Orders> GetAll()
        {
            var orders = new List<Orders>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT OrderID, ProductID, SupplierID, UserID, OrderDate, Quantity, Status FROM Orders";
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Orders
                        {
                            OrderID = Convert.ToInt32(reader["OrderID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Status = reader["Status"].ToString(),
                        });
                    }
                }
            }
            return orders;
        }

        public Orders Insert(Orders orders)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Orders (ProductID, SupplierID, UserID, Quantity, Status, OrderDate) OUTPUT inserted.OrderID VALUES (@ProductID, @SupplierID, @UserID, @Quantity, @Status, @OrderDate)";
                command.Parameters.AddWithValue("@ProductID", orders.ProductID);
                command.Parameters.AddWithValue("@SupplierID", orders.SupplierID);
                command.Parameters.AddWithValue("@UserID", orders.UserID);
                command.Parameters.AddWithValue("@Quantity", orders.Quantity);
                command.Parameters.AddWithValue("@Status", orders.Status);
                command.Parameters.AddWithValue("@OrderDate", orders.OrderDate);

                connection.Open();
                orders.OrderID = Convert.ToInt32(command.ExecuteScalar());
            }
            return orders;
        }

        
        public List<Orders> GetActiveOrdersByUserId(int userId)
        {
            var orders = new List<Orders>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Orders WHERE UserID = @UserId AND Status = 'Pending'";
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Orders
                        {
                            OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"))
                        });
                    }
                }
            }
            return orders;
        }

      
        public Orders GetById(int orderId)
        {
            Orders order = null;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT OrderID, ProductID, SupplierID, UserID, OrderDate, Quantity, Status FROM Orders WHERE OrderID = @OrderID";
                command.Parameters.AddWithValue("@OrderID", orderId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        order = new Orders
                        {
                            OrderID = Convert.ToInt32(reader["OrderID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Status = reader["Status"].ToString(),
                        };
                    }
                }
            }
            return order;
        }

        public Orders Update(Orders orders)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Orders SET ProductID = @ProductID, SupplierID = @SupplierID, UserID = @UserID, Quantity = @Quantity, Status = @Status, OrderDate = @OrderDate WHERE OrderID = @OrderID";
                command.Parameters.AddWithValue("@OrderID", orders.OrderID);
                command.Parameters.AddWithValue("@ProductID", orders.ProductID);
                command.Parameters.AddWithValue("@SupplierID", orders.SupplierID);
                command.Parameters.AddWithValue("@UserID", orders.UserID);
                command.Parameters.AddWithValue("@Quantity", orders.Quantity);
                command.Parameters.AddWithValue("@Status", orders.Status);
                command.Parameters.AddWithValue("@OrderDate", orders.OrderDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
            return orders;
        }

        public void Delete(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Orders WHERE OrderID = @OrderID";
                command.Parameters.AddWithValue("@OrderID", orderId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
