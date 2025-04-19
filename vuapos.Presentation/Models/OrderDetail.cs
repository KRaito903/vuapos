using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Views.Product;

namespace vuapos.Presentation.Models
{
    public class OrderDetail
    {

        public string OrderDetail_Id { get; set; }
        public string Order_Id { get; set; } // Khóa ngoại liên kết đến Order
        public string Product_Id { get; set; } // Khóa ngoại liên kết đến Product
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // Giá tại thời điểm đặt hàng
        public decimal Subtotal { get; set; } // Quantity * UnitPrice

        // Thuộc tính Navigation (nếu bạn sử dụng ORM như Entity Framework)
        public Product Product { get; set; }
        // public Order Order { get; set; } // Tránh vòng lặp nếu không cần thiết

        public OrderDetail() { }

        public OrderDetail(Product product, int quantity)
        {
            Product_Id = product.Product_Id;
            Quantity = quantity;
            UnitPrice = product.Price;
            Subtotal = Quantity * UnitPrice;
        }

    }
}
