using Dal.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Dal.Concrete
{
    public class UsersDal : IUsersDal
    {
        private readonly SqlConnection _connection;

        public UsersDal(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }




        private string HashPassword(string password, string salt)
        {
            using (var sha512 = SHA512.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(salt + password);
                byte[] hashBytes = sha512.ComputeHash(passwordBytes);
                return salt + Convert.ToBase64String(hashBytes);
            }
        }


       

        public Users GetUserByUsername(string username)
        {
            Users user = null;

            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Users WHERE Username = @Username";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Username", username);

                try
                {
                    _connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new Users
                            {
                                UserID = (int)reader["UserID"],
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                            };
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"SQL error: {ex.Message}");
                }
                finally
                {
                    _connection.Close();
                }
            }

            return user;
        }

        public Users GetUserByUsernameAndPassword(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user == null)
            {
                
                Console.WriteLine($"User not found: {username}");
                return null;
            }

            if (!VerifyPassword(password, user.Password))
            {
               
                Console.WriteLine($"Invalid password for user: {username}");
                return null;
            }

            return user;
        }

        public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            string salt = storedHashedPassword.Substring(0, 24);
            string hashedInputPassword = HashPassword(enteredPassword, salt);
            return hashedInputPassword == storedHashedPassword;
        }
      

        public List<Users> GetAll()
        {
            var users = new List<Users>();
            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT UserID, Username, Password FROM Users";

                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new Users
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                        });
                    }
                }
                _connection.Close();
            }
            return users;
        }

        public Users Insert(Users user)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(user.Password, salt);

            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Users (Username, Password) OUTPUT INSERTED.UserID VALUES (@Username, @Password)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                _connection.Open();
                user.UserID = Convert.ToInt32(command.ExecuteScalar());
                _connection.Close();
                return user;
            }
        }



        public void UpdatePassword(Users user)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(user.Password, salt);

            using (SqlCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Password", hashedPassword);
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
            Users user = null;

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
                        user = new Users
                        {
                            UserID = (int)reader["UserID"],
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                        };
                    }
                }
                _connection.Close();
            }

            return user;
        }
    }
}
