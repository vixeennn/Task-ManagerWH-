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
        //public void AddSupplier_ShouldReturnAddedSupplier()
        //{
        //    // Arrange
        //    var supplier = new Suppliers { SupplierID = 1, Name = "New Supplier" };
        //    _mockSuppliersDal.Setup(dal => dal.Insert(It.IsAny<Suppliers>())).Returns(supplier);

        //    // Act
        //    var result = _suppliersManager.AddSupplier(supplier);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.SupplierID);
        //    Assert.AreEqual("New Supplier", result.Name);
        //}


        public void AddSupplier_ShouldReturnAddedSupplier_WithGeneratedID()
        {
            // Arrange
            var supplierToAdd = new Suppliers { Name = "New Supplier" }; // Не вказуємо SupplierID
            var expectedSupplier = new Suppliers { SupplierID = 1, Name = "New Supplier" }; // Це значення ми очікуємо після додавання

            // Налаштовуємо мок-об'єкт так, щоб Insert повертав вже присвоєний ID постачальнику
            _mockSuppliersDal.Setup(dal => dal.Insert(It.IsAny<Suppliers>()))
                .Returns((Suppliers supplier) =>
                {
                    // Присвоюємо ID новому постачальнику під час додавання
                    supplier.SupplierID = 1;
                    return supplier;
                });

            // Act
            var result = _suppliersManager.AddSupplier(supplierToAdd);

            // Assert
            Assert.IsNotNull(result); // Перевіряємо, що результат не null
            Assert.AreEqual(1, result.SupplierID); // Перевіряємо, чи ID був правильно присвоєний
            Assert.AreEqual("New Supplier", result.Name); // Перевіряємо, чи правильне ім'я
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
