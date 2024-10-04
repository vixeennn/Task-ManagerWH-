
using Dal.Concrete;
using DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DalTest
{

    
        public class OrdersDalTests
        {
            private readonly OrdersDal _dal;
            private List<Orders> _ordersInDb = new List<Orders>();

            public OrdersDalTests()
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("manager.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("ManagerWH") ?? "";
                _dal = new OrdersDal(connectionString);
            }

            [SetUp]
            public void Setup()
            {
                var order1 = new Orders { ProductID = 1, SupplierID = 1, UserID = 1, Quantity = 10, Status = "Pending", OrderDate = DateTime.Now };
                _ordersInDb.Add(_dal.Insert(order1));

                var order2 = new Orders { ProductID = 2, SupplierID = 2, UserID = 2, Quantity = 20, Status = "Pending", OrderDate = DateTime.Now };
                _ordersInDb.Add(_dal.Insert(order2));

                var order3 = new Orders { ProductID = 3, SupplierID = 3, UserID = 3, Quantity = 30, Status = "Pending", OrderDate = DateTime.Now };
                _ordersInDb.Add(_dal.Insert(order3));
            }

            [TearDown]
            public void TearDown()
            {
                foreach (var order in _ordersInDb)
                {
                    _dal.Delete(order.OrderID);
                }
                _ordersInDb.Clear();
            }

           

        [Test]
            public void GetByIdTest()
            {
                var insertedOrder = _ordersInDb.First();
                var order = _dal.GetById(insertedOrder.OrderID);

                if (order == null || insertedOrder.OrderID != order.OrderID)
                {
                    throw new Exception("GetByIdTest failed: Order not found or ID mismatch.");
                }
            }

            [Test]
            public void UpdateTest()
            {
                var insertedOrder = _ordersInDb.First();
                insertedOrder.Quantity = 50; 

                var updatedOrder = _dal.Update(insertedOrder);

                if (updatedOrder.Quantity != 50)
                {
                    throw new Exception("UpdateTest failed: Quantity not updated.");
                }
            }

            [Test]
            public void DeleteTest()
            {
                var insertedOrder = _ordersInDb.First();

                _dal.Delete(insertedOrder.OrderID);
                var deletedOrder = _dal.GetById(insertedOrder.OrderID);

                if (deletedOrder != null)
                {
                    throw new Exception("DeleteTest failed: Order still exists.");
                }
            }
        }
    


}




