using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Customer;
using vuapos.Presentation.DTO.Staff;
using vuapos.Presentation.Models;
using vuapos.Presentation.Views.Customer;

namespace vuapos.Presentation.Services
{
    public class StaffService : ApiService
    {

        public StaffService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjBjYjU1MmIwLTQxNTItNDA3NC1hYmVmLTFiMmQwZTU2ZmI0NCIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ0ODc2MzcyLCJleHAiOjE3NDU0ODExNzJ9.Xpw9mgkAu7WXirZz1dRxYTgCHULA2-ntevPXpaXIKDM";
        }

        public async Task<List<Staff>?> GetAllStaffsAsync()
        {
            return await SendRequestAsync<List<Staff>>(HttpMethod.Get, "staff");
        }

        public async Task<Staff?> GetStaffByIdAsync(string id)
        {
            return await SendRequestAsync<Staff>(HttpMethod.Get, $"staff/{id}");
        }
        public async Task<bool> CreateStaffAsync(StaffCreateDTO staff)
        {
            var response = await SendRequestAsync<Staff>(HttpMethod.Post, "staff", staff);
            return response != null;
        }
        public async Task<bool> UpdateStaffAsync(string staffId, object updateData)
        {
            var response = await SendRequestAsync<Staff>(HttpMethod.Patch, $"staff/{staffId}", updateData);
            return response != null;
        }

        public async Task<bool> DeleteStaffAsync(string staffId)
        {
            var response = await SendRequestAsync<object>(HttpMethod.Delete, $"staff/{staffId}");
            return response != null;
        }


    }
}
