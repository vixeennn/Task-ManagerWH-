using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Products
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int QuantityInStock { get; set; }
        public decimal Price { get; set; }
    }
}
