using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.Views.Product
{
    public class Product
    {
        public string Product_Id { get; set; } = string.Empty;
        public string Product_Name { get; set; } = string.Empty;
        public int Stock_Quantity { get; set; }
        public string Category_Id { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public decimal Cost_Price { get; set; }
        public string Image_Path { get; set; } = string.Empty;
        public string Category_Name { get; set; } = string.Empty;
    }

}
