using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using BusinessLogic.Concrete;
using Dal.Interface;
using DTO;

namespace BLTests
{
    [TestFixture]
    public class SuppliersManagerTest
    {
        private SuppliersManager _suppliersManager;
        private Mock<ISuppliersDal> _mockSuppliersDal;

        [SetUp]
        public void Setup()
        {
            _mockSuppliersDal = new Mock<ISuppliersDal>();
            _suppliersManager = new SuppliersManager(_mockSuppliersDal.Object);
        }

        [Test]
        public void GetAllSuppliers_ShouldReturnAllSuppliers()
        {
            // Arrange
            var suppliersList = new List<Suppliers> { new Suppliers { SupplierID = 1, Name = "Supplier1" } };
            _mockSuppliersDal.Setup(dal => dal.GetAll()).Returns(suppliersList);

            // Act
            var result = _suppliersManager.GetAllSuppliers();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Supplier1", result[0].Name);
        }

        [Test]
        public void AddSupplier_ShouldReturnAddedSupplier()
        {
            // Arrange
            var supplier = new Suppliers { SupplierID = 1, Name = "New Supplier" };
            _mockSuppliersDal.Setup(dal => dal.Insert(It.IsAny<Suppliers>())).Returns(supplier);

            // Act
            var result = _suppliersManager.AddSupplier(supplier);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.SupplierID);
            Assert.AreEqual("New Supplier", result.Name);
        }

        [Test]
        public void DeleteSupplier_ShouldInvokeDeleteOnDal()
        {
            // Arrange
            int supplierId = 2;

            // Act
            _suppliersManager.DeleteSupplier(supplierId);

            // Assert
            _mockSuppliersDal.Verify(dal => dal.Delete(supplierId), Times.Once);
        }
    }
}
