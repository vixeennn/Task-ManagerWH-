using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using BusinessLogic.Concrete;
using Dal.Interface;
using DTO;

namespace BLTests
{
    [TestFixture]
    public class OrdersManagerTests
    {
        private OrdersManager _ordersManager;
        private Mock<IOrdersDal> _mockOrdersDal;

        [SetUp]
        public void Setup()
        {
            _mockOrdersDal = new Mock<IOrdersDal>();
            _ordersManager = new OrdersManager(_mockOrdersDal.Object);
        }

        [Test]
        public void GetAllOrders_ShouldReturnOrdersList()
        {
            // Arrange
            var ordersList = new List<Orders> { new Orders { OrderID = 1, Status = "New" } };
            _mockOrdersDal.Setup(dal => dal.GetAll()).Returns(ordersList);

            // Act
            var result = _ordersManager.GetAllOrders();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].OrderID);
        }

        [Test]
        public void AddOrder_ShouldAddOrderAndReturnOrder()
        {
            // Arrange
            var order = new Orders { OrderID = 2, Status = "Pending" };
            _mockOrdersDal.Setup(dal => dal.Insert(It.IsAny<Orders>())).Returns(order);

            // Act
            var result = _ordersManager.AddOrder(order);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.OrderID);
        }

        [Test]
        public void GetActiveOrdersByUserId_ShouldReturnOrdersForSpecificUser()
        {
            // Arrange
            int userId = 1;
            var ordersList = new List<Orders> { new Orders { OrderID = 3, UserID = userId, Status = "Active" } };
            _mockOrdersDal.Setup(dal => dal.GetActiveOrdersByUserId(userId)).Returns(ordersList);

            // Act
            var result = _ordersManager.GetActiveOrdersByUserId(userId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(userId, result[0].UserID);
        }

        [Test]
        public void UpdateOrder_ShouldUpdateAndReturnOrder()
        {
            // Arrange
            var order = new Orders { OrderID = 4, Status = "Shipped" };
            _mockOrdersDal.Setup(dal => dal.Update(It.IsAny<Orders>())).Returns(order);

            // Act
            var result = _ordersManager.UpdateOrder(order);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Shipped", result.Status);
        }

        [Test]
        public void DeleteOrder_ShouldDeleteOrder()
        {
            // Arrange
            int orderId = 5;

            // Act
            _ordersManager.DeleteOrder(orderId);

            // Assert
            _mockOrdersDal.Verify(dal => dal.Delete(orderId), Times.Once);
        }
    }
}
