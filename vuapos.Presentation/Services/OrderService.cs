using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Order;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.Services
{
    public class OrderService : ApiService
    {
        public OrderService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjBjYjU1MmIwLTQxNTItNDA3NC1hYmVmLTFiMmQwZTU2ZmI0NCIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ0ODc2MzcyLCJleHAiOjE3NDU0ODExNzJ9.Xpw9mgkAu7WXirZz1dRxYTgCHULA2-ntevPXpaXIKDM";
        }
        public async Task<List<Order>?> GetAllOrdersAsync()
        {
            return await SendRequestAsync<List<Order>>(HttpMethod.Get, "order");
        }

        public async Task<bool> CreateOrder(OrderCreateDTO orderData)
        {
           var result = await SendRequestAsync<Order>(HttpMethod.Post,"order",orderData);
           return result != null;
        }

        public async Task<bool> CreateOrderDetail(OrderDetailCreateDTO orderDetailData)
        {
            var result = await SendRequestAsync<OrderDetail>(HttpMethod.Post, "order-detail", orderDetailData);
            return result != null;
        }
    }
}
