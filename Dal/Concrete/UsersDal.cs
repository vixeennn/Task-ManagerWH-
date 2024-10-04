using Dal.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dal.Concrete
{
    public class UsersDal : IUsersDal
    {
        private readonly SqlConnection _connection;

        public UsersDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public List<Users> GetAll()
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT UserID, Username, Password FROM Users";

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                var users = new List<Users>();
                while (reader.Read())
                {
                    users.Add(new Users
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                    });
                }

                _connection.Close();
                return users;
            }
        }

        public Users Insert(Users users)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Users (Username, Password) OUTPUT INSERTED.UserID VALUES (@Username, @Password)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Username", users.Username);
                command.Parameters.AddWithValue("@Password", users.Password);

                _connection.Open();
                users.UserID = Convert.ToInt32(command.ExecuteScalar());
                _connection.Close();
                return users;
            }
        }

        public Users GetUserByUsernameAndPassword(string username, string password)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT UserID, Username, Password FROM Users WHERE Username = @Username AND Password = @Password";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Users
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                    };
                }

                _connection.Close();
                return null;
            }
        }

        public Users GetUserByUsername(string username)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Users WHERE Username = @Username";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Username", username);

                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Users
                        {
                            UserID = (int)reader["UserID"],
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                        };
                    }
                }
                _connection.Close();
            }

            return null;
        }

        public void UpdatePassword(Users user)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@UserID", user.UserID);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Delete(int userId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Users WHERE UserID = @UserID";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@UserID", userId);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public Users GetById(int userId)
        {
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Users WHERE UserID = @UserID";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@UserID", userId);

                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Users
                        {
                            UserID = (int)reader["UserID"],
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                        };
                    }
                }
                _connection.Close();
            }

            return null;
        }
    }
}
