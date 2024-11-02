using BusinessLogic.Interface;
using Dal.Interface;
using DTO;


namespace BusinessLogic.Concrete
{
    
    public class OrdersManager : IOrdersManager
    {
        private readonly IOrdersDal _ordersDal;

        public OrdersManager(IOrdersDal ordersDal)
        {
            _ordersDal = ordersDal;
        }

        public List<Orders> GetAllOrders()
        {
            return _ordersDal.GetAll();
        }

        public Orders AddOrder(Orders order)
        {
            return _ordersDal.Insert(order);
        }

        public List<Orders> GetActiveOrdersByUserId(int userId)
        {
            return _ordersDal.GetActiveOrdersByUserId(userId);
        }

        public Orders GetOrderById(int orderId)
        {
            return _ordersDal.GetById(orderId);
        }

        public Orders UpdateOrder(Orders order)
        {
            return _ordersDal.Update(order);
        }

        public void DeleteOrder(int orderId)
        {
            _ordersDal.Delete(orderId);
        }
    }
}
