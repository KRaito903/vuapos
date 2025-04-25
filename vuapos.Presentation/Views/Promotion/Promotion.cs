using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.Views.Promotion
{
    public class Promotion
    {
        public string Promotion_Id { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Discount_Percentage { get; set; } = String.Empty;
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
    }
}
