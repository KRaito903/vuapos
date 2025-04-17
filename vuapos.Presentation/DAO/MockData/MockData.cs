using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.DAO.MockData
{
    public class MockData
    {
        public static List<Product> Products = new List<Product>()
            {
                new Product { Product_Id = "1", ProductName = "Áo thun basic trắng", Price = 120000, StockQuantity = 100, Brand = "Local Brand" },
                new Product { Product_Id = "2", ProductName = "Quần jean ống rộng", Price = 250000, StockQuantity = 50, Brand = "H&M" },
                new Product { Product_Id = "3", ProductName = "Váy hoa nhí", Price = 180000, StockQuantity = 75, Brand = "Zara" }
            };

        public static List<Order> Orders = new List<Order>()
            {
                new Order { Order_Id = "101", OrderDate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"), CustomerName = "Nguyễn Văn A", TotalAmount = 370000, OrderStatus = "Đã giao" },
                new Order { Order_Id = "102", OrderDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), CustomerName = "Trần Thị B", TotalAmount = 120000, OrderStatus = "Đang xử lý" }
            };

        public static List<OrderDetail> OrderDetails = new List<OrderDetail>()
            {
                new OrderDetail { OrderDetail_Id = "1", Order_Id = "101", Product_Id = "1", Quantity = 1, UnitPrice = 120000, Subtotal = 120000 },
                new OrderDetail { OrderDetail_Id = "2", Order_Id = "101", Product_Id = "2", Quantity = 1, UnitPrice = 250000, Subtotal = 250000 },
                new OrderDetail { OrderDetail_Id = "3", Order_Id = "102", Product_Id = "1", Quantity = 1, UnitPrice = 120000, Subtotal = 120000 }
            };
        public static List<ProductVariant> ProductVariants = new List<ProductVariant>()
            {
                new ProductVariant { Variant_Id = "1", Product_Id = "1", Size = "M", Color = "Trắng", StockQuantity = 50 },
                new ProductVariant { Variant_Id = "2", Product_Id = "1", Size = "L", Color = "Đen", StockQuantity = 30 },
                new ProductVariant { Variant_Id = "3", Product_Id = "2", Size = "M", Color = "Xanh", StockQuantity = 20 }
            };
    }
}
