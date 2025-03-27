using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Customer;
using vuapos.Presentation.Services;
using vuapos.Presentation.Views.Customer;

namespace vuapos.Presentation.Models
{
    public class CustomerViewModel
    {
        private readonly CustomerService _customerService;

        public ObservableCollection<Customer> Customers { get; set; } = new();

        public CustomerViewModel()
        {
            _customerService = App.Services.GetRequiredService<CustomerService>();
        }

        // Load customers from API
        public async Task LoadCustomersAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            if (customers != null)
            {
                Customers.Clear();
                foreach (var customer in customers)
                    Customers.Add(customer);
            }
        }

        // Add a new customer
        public async Task AddCustomerAsync(string name, string phone, string email)
        {
            var newCustomer = new CustomerCreateDTO
            {
                name = name,
                phone = phone,
                email = email
            };
            var addedCustomer = await _customerService.CreateCustomerAsync(newCustomer);

            if (addedCustomer != null)
                Customers.Add(addedCustomer);
        }

        // Update existing customer
        public async Task UpdateCustomerAsync(Customer customer, CustomerUpdateDTO updateDto)
        {
            var updatedCustomer = await _customerService.UpdateCustomerAsync(customer.Customer_Id, updateDto);

            if (updatedCustomer != null)
            {
                // Update customer in the list
                await this.LoadCustomersAsync();
            }
        }

        // Delete customer
        public async Task DeleteCustomerAsync(Customer customer)
        {
            bool success = await _customerService.DeleteCustomerAsync(customer.Customer_Id);
            if (success)
            {
                Customers.Remove(customer);
            }
        }

        // Fetch customer details
        public async Task<Customer?> GetCustomerByIdAsync(string customerId)
        {
            return await _customerService.GetCustomerByIdAsync(customerId);
        }
    }
}
