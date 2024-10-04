

namespace DTO
{
    public class Orders
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int SupplierID { get; set; }
        public int UserID { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
