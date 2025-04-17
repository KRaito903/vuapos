using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.Models
{
    public class Order
    {
        public string Order_Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerMail { get; set; }
        public string StaffID { get; set; }
        public string OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public ObservableCollection<OrderDetail> OrderDetails { get; set; } = new ObservableCollection<OrderDetail>();
         // Các thuộc tính khác của đơn hàng như CustomerId, ShippingAddress, v.v.
    }

}
