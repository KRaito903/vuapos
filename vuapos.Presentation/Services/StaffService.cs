using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Customer;
using vuapos.Presentation.Models;
using vuapos.Presentation.Views.Customer;

namespace vuapos.Presentation.Services
{
    public class StaffService : ApiService
    {

        public StaffService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjlmODNkNjlhLWVjMzktNDUyMi1hMzhlLWM2MTM5OWQ2NzJiOCIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQzMDU1MTEzLCJleHAiOjE3NDM2NTk5MTN9.3qliEWO_bzjChVBsgy0pxFxwyR9SuUwl_KXaC19LWHw";
        }

        public async Task<List<Staff>?> GetAllStaffsAsync()
        {
            return await SendRequestAsync<List<Staff>>(HttpMethod.Get, "staff");
        }

    }
}
