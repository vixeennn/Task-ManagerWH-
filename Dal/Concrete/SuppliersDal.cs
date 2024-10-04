using Dal.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dal.Concrete
{
    public class SuppliersDal : ISuppliersDal
    {
        private readonly SqlConnection _connection;

        public SuppliersDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public List<Suppliers> GetAll()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT SupplierID, Name, Phone, Address FROM Suppliers";

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                var suppliers = new List<Suppliers>();
                while (reader.Read())
                {
                    suppliers.Add(new Suppliers
                    {
                        SupplierID = Convert.ToInt32(reader["SupplierID"]),
                        Name = reader["Name"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString(),
                    });
                }

                _connection.Close();
                return suppliers;
            }
        }

        public Suppliers Insert(Suppliers suppliers)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Suppliers (Name, Phone, Address) OUTPUT INSERTED.SupplierID VALUES (@Name, @Phone, @Address)";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Name", suppliers.Name);
                command.Parameters.AddWithValue("@Phone", suppliers.Phone);
                command.Parameters.AddWithValue("@Address", suppliers.Address);

                _connection.Open();
                suppliers.SupplierID = Convert.ToInt32(command.ExecuteScalar());
                _connection.Close();

                return suppliers;
            }
        }

        public void Delete(int supplierId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Suppliers WHERE SupplierID = @SupplierID";
                command.Parameters.AddWithValue("@SupplierID", supplierId);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}
