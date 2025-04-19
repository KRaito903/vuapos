using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Order
{
    public class OrderDetailCreateDTO
    {
        public required String order_id { get; set; }
        public required String product_id { get; set; }
        public required Double price { get; set; }
        public required int quantity { get; set; }
    }
}
