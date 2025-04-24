using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Customer;
using vuapos.Presentation.Views.Customer;

namespace vuapos.Presentation.Services
{
    public class CustomerService : ApiService
    {
        public CustomerService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjBjYjU1MmIwLTQxNTItNDA3NC1hYmVmLTFiMmQwZTU2ZmI0NCIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ1NDkyNTk4LCJleHAiOjE3NDYwOTczOTh9._dhH4UZRNzp70jKeWus61XnMZ7Nt6lZWUwr-e2lNMds";
        }

        public async Task<List<Customer>?> GetAllCustomersAsync()
        {
            return await SendRequestAsync<List<Customer>>(HttpMethod.Get, "customer");
        }

        public async Task<Customer?> GetCustomerByIdAsync(string customerId)
        {
            return await SendRequestAsync<Customer>(HttpMethod.Get, $"customer/{customerId}");
        }

        public async Task<Customer?> CreateCustomerAsync(CustomerCreateDTO customer)
        {
            return await SendRequestAsync<Customer>(HttpMethod.Post, "customer", customer);
        }

        public async Task<Customer?> UpdateCustomerAsync(string customerId, object updateData)
        {
            return await SendRequestAsync<Customer>(HttpMethod.Patch, $"customer/{customerId}", updateData);
        }

        public async Task<bool> DeleteCustomerAsync(string customerId)
        {
            var response = await SendRequestAsync<object>(HttpMethod.Delete, $"customer/{customerId}");
            return response != null;
        }
    }
}
