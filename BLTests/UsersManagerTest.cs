using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using BusinessLogic.Concrete;
using Dal.Interface;
using DTO;

namespace BLTests
{
    [TestFixture]
    public class UsersManagerTest
    {
        private UsersManager _usersManager;
        private Mock<IUsersDal> _mockUsersDal;

        [SetUp]
        public void SetUp()
        {
            _mockUsersDal = new Mock<IUsersDal>();
            _usersManager = new UsersManager(_mockUsersDal.Object);
        }

        [Test]
        public void GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<Users>
            {
                new Users { UserID = 1, Username = "user1" },
                new Users { UserID = 2, Username = "user2" }
            };
            _mockUsersDal.Setup(dal => dal.GetAll()).Returns(users);

            // Act
            var result = _usersManager.GetAllUsers();

            // Assert
            Assert.AreEqual(users, result);
            _mockUsersDal.Verify(dal => dal.GetAll(), Times.Once);
        }

        [Test]
        //public void AddUser_CallsInsertMethodWithCorrectUser()
        //{
        //    // Arrange
        //    var user = new Users { UserID = 1, Username = "newuser" };
        //    _mockUsersDal.Setup(dal => dal.Insert(user)).Returns(user);

        //    // Act
        //    var result = _usersManager.AddUser(user);

        //    // Assert
        //    Assert.AreEqual(user, result);
        //    _mockUsersDal.Verify(dal => dal.Insert(user), Times.Once);
        //}


        public void AddUser_CallsInsertMethodWithCorrectUser_WithGeneratedID()
        {
            // Arrange
            var userToAdd = new Users { Username = "newuser" }; 
            var expectedUser = new Users { UserID = 1, Username = "newuser" };

            
            _mockUsersDal.Setup(dal => dal.Insert(It.IsAny<Users>()))
                .Returns((Users user) =>
                {
                   
                    user.UserID = 1; 
                    return user;
                });

            // Act
            var result = _usersManager.AddUser(userToAdd);

            // Assert
            Assert.IsNotNull(result); 
            Assert.AreEqual(1, result.UserID); 
            Assert.AreEqual("newuser", result.Username); 
            _mockUsersDal.Verify(dal => dal.Insert(It.IsAny<Users>()), Times.Once); 
        }

        [Test]
        public void GetUserByUsernameAndPassword_ReturnsCorrectUser()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var user = new Users { UserID = 1, Username = username };
            _mockUsersDal.Setup(dal => dal.GetUserByUsernameAndPassword(username, password)).Returns(user);

            // Act
            var result = _usersManager.GetUserByUsernameAndPassword(username, password);

            // Assert
            Assert.AreEqual(user, result);
            _mockUsersDal.Verify(dal => dal.GetUserByUsernameAndPassword(username, password), Times.Once);
        }

        [Test]
        public void GetUserByUsername_ReturnsCorrectUser()
        {
            // Arrange
            var username = "testuser";
            var user = new Users { UserID = 1, Username = username };
            _mockUsersDal.Setup(dal => dal.GetUserByUsername(username)).Returns(user);

            // Act
            var result = _usersManager.GetUserByUsername(username);

            // Assert
            Assert.AreEqual(user, result);
            _mockUsersDal.Verify(dal => dal.GetUserByUsername(username), Times.Once);
        }

        [Test]
        public void GetUserById_ReturnsCorrectUser()
        {
            // Arrange
            var userId = 1;
            var user = new Users { UserID = userId, Username = "testuser" };
            _mockUsersDal.Setup(dal => dal.GetById(userId)).Returns(user);

            // Act
            var result = _usersManager.GetUserById(userId);

            // Assert
            Assert.AreEqual(user, result);
            _mockUsersDal.Verify(dal => dal.GetById(userId), Times.Once);
        }

        [Test]
        public void UpdateUserPassword_CallsUpdatePasswordMethod()
        {
            // Arrange
            var user = new Users { UserID = 1, Username = "testuser" };

            // Act
            _usersManager.UpdateUserPassword(user);

            // Assert
            _mockUsersDal.Verify(dal => dal.UpdatePassword(user), Times.Once);
        }

        [Test]
        public void DeleteUser_CallsDeleteMethodWithCorrectUserId()
        {
            // Arrange
            var userId = 1;

            // Act
            _usersManager.DeleteUser(userId);

            // Assert
            _mockUsersDal.Verify(dal => dal.Delete(userId), Times.Once);
        }

        [Test]
        public void GetCurrentUserId_ReturnsCorrectUserId()
        {
            // Arrange
            var username = "testuser";
            var user = new Users { UserID = 1, Username = username };
            _mockUsersDal.Setup(dal => dal.GetUserByUsername(username)).Returns(user);

            // Act
            var result = _usersManager.GetCurrentUserId(username);

            // Assert
            Assert.AreEqual(1, result);
            _mockUsersDal.Verify(dal => dal.GetUserByUsername(username), Times.Once);
        }

        [Test]
        public void GetCurrentUserId_ReturnsZeroWhenUserNotFound()
        {
            // Arrange
            var username = "nonexistentuser";
            _mockUsersDal.Setup(dal => dal.GetUserByUsername(username)).Returns((Users)null);

            // Act
            var result = _usersManager.GetCurrentUserId(username);

            // Assert
            Assert.AreEqual(0, result);
            _mockUsersDal.Verify(dal => dal.GetUserByUsername(username), Times.Once);
        }
    }
}
