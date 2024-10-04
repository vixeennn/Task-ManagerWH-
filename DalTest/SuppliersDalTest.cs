using Dal.Concrete;
using DTO;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DalTest
{



   
        [TestFixture]
        public class SuppliersDalTests
        {
            private SuppliersDal _dal;
            private List<Suppliers> _suppliersInDb;

            [SetUp]
            public void Setup()
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("manager.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("ManagerWH") ?? "";
                _dal = new SuppliersDal(connectionString);

               
                _suppliersInDb = new List<Suppliers>
            {
                _dal.Insert(new Suppliers { Name = "Supplier 1", Phone = "123456789", Address = "Address 1" }),
                _dal.Insert(new Suppliers { Name = "Supplier 2", Phone = "987654321", Address = "Address 2" }),
                _dal.Insert(new Suppliers { Name = "Supplier 3", Phone = "111222333", Address = "Address 3" })
            };
            }

            [TearDown]
            public void TearDown()
            {
                foreach (var supplier in _suppliersInDb)
                {
                    _dal.Delete(supplier.SupplierID);
                }
                _suppliersInDb.Clear();
            }

            [Test]
            public void InsertTest()
            {
                var newSupplier = new Suppliers { Name = "Supplier 4", Phone = "444555666", Address = "Address 4" };
                var insertedSupplier = _dal.Insert(newSupplier);

                Assert.That(insertedSupplier.SupplierID, Is.GreaterThan(0), "InsertTest failed: Supplier ID should not be zero.");

                
                _dal.Delete(insertedSupplier.SupplierID);
            }

            [Test]
            public void GetAllTest()
            {
                var suppliers = _dal.GetAll();

                Assert.That(suppliers.Count, Is.GreaterThanOrEqualTo(3), "GetAllTest failed: Expected at least 3 suppliers.");
                Assert.That(suppliers.Any(s => s.Name == "Supplier 1"), Is.True, "GetAllTest failed: Supplier 1 not found.");
            }

            [Test]
            public void DeleteTest()
            {
                var supplierToDelete = _suppliersInDb.First();
                _dal.Delete(supplierToDelete.SupplierID);
                var deletedSupplier = _dal.GetAll().FirstOrDefault(s => s.SupplierID == supplierToDelete.SupplierID);

                Assert.That(deletedSupplier, Is.Null, "DeleteTest failed: Supplier still exists after deletion.");
            }
        }
    



}
