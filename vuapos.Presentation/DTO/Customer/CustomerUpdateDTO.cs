using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Customer
{
    public class CustomerUpdateDTO
    {
        public string? name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public int? point { get; set; }
    }
}
