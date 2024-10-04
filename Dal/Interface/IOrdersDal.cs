using DTO;

namespace Dal.Interface
{
    public interface IOrdersDal
    {
        List<Orders> GetAll();
        Orders Insert(Orders orders);
        List<Orders> GetActiveOrdersByUserId(int userId);
        Orders GetById(int orderId);
        Orders Update(Orders orders);
        void Delete(int orderId);
    }
}
