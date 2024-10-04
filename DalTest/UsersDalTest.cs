using Dal.Concrete;
using DTO;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DalTest
{

    

    
        [TestFixture]
        public class UsersDalTests
        {
            private UsersDal _dal;
            private List<Users> _usersInDb;

            [OneTimeSetUp]
            public void OneTimeSetup()
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("manager.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("ManagerWH") ?? "";
                _dal = new UsersDal(connectionString);
            }

            [SetUp]
            public void Setup()
            {
                _usersInDb = new List<Users>
            {
                _dal.Insert(new Users { Username = "User1", Password = "Password1" }),
                _dal.Insert(new Users { Username = "User2", Password = "Password2" }),
                _dal.Insert(new Users { Username = "User3", Password = "Password3" })
            };
            }

            [TearDown]
            public void TearDown()
            {
                foreach (var user in _usersInDb)
                {
                    _dal.Delete(user.UserID);
                }
                _usersInDb.Clear();
            }

            [Test]
            public void InsertUserTest()
            {
                var newUser = new Users { Username = "User4", Password = "Password4" };
                var insertedUser = _dal.Insert(newUser);

                Assert.That(insertedUser.UserID, Is.GreaterThan(0), "InsertUserTest failed: User ID should not be zero.");

               
                _dal.Delete(insertedUser.UserID);
            }

            [Test]
            public void GetAllUsersTest()
            {
                var users = _dal.GetAll();

                Assert.That(users.Count, Is.GreaterThanOrEqualTo(3), "GetAllUsersTest failed: Expected at least 3 users.");
                Assert.That(users.Any(u => u.Username == "User1"), Is.True, "GetAllUsersTest failed: User1 not found.");
            }




        }
    
}
        