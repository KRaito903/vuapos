using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Views.Category;

namespace vuapos.Presentation.Models
{
    public class Product
    {
        public string Product_Id { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public string CategoryId { get; set; } = string.Empty; // Khóa ngoại liên kết đến Category
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public decimal CostPrice { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
    }
}