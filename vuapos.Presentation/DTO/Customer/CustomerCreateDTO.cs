using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DTO.Customer
{
    public class CustomerCreateDTO
    {
        public required String name { get; set; }
        public required String phone { get; set; }
        public required String email { get; set; }

    }
}
