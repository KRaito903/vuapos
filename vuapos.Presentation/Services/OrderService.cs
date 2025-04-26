using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.DTO.Order;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.Views.Customer;

namespace vuapos.Presentation.Services
{
    public class OrderService : ApiService
    {
        public OrderService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = App.Services!.GetRequiredService<IUserSession>().Token;
        }
        public async Task<Response<Order>?> GetAllOrdersAsync(int page)
        {
            return await SendRequestAsync<Response<Order>>(HttpMethod.Get, $"order?page={page}");
        }


        public async Task<Response<Order>?> GetCustomerOrderByDate(string customerId, string startDate, string endDate)
        {
            return await SendRequestAsync<Response<Order>>(HttpMethod.Get, $"order?search={customerId}&startDate={startDate}&endDate={endDate}");
        }


        public async Task<bool> CreateOrder(OrderCreateDTO orderData)
        {
           var result = await SendRequestAsync<Order>(HttpMethod.Post, "order", orderData);
           return result != null;
        }

        public async Task<bool> CreateOrderDetail(OrderDetailCreateDTOList orderDetailData)
        {
            var result = await SendRequestAsync<OrderDetail>(HttpMethod.Post, "order-detail", orderDetailData);
            return result != null;
        }
    }
}
