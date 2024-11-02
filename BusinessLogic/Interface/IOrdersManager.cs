using DTO;

namespace BusinessLogic.Interface
{
    public interface IOrdersManager
    {
        List<Orders> GetAllOrders();
        Orders AddOrder(Orders order);
        List<Orders> GetActiveOrdersByUserId(int userId);
        Orders GetOrderById(int orderId);
        Orders UpdateOrder(Orders order);
        void DeleteOrder(int orderId);
    }
}




