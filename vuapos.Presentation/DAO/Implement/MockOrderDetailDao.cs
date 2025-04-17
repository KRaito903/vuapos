using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DAO.Interface;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.DAO.Implement
{
    public class MockOrderDetailDao : IOrderDetailDao
    {
        private readonly List<OrderDetail> _orderDetails = new List<OrderDetail>(); // Giả lập database

        public MockOrderDetailDao() {

            _orderDetails = MockData.MockData.OrderDetails; // Giả lập dữ liệu

        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(string orderDetail_Id)
        {
            return await Task.FromResult(_orderDetails.FirstOrDefault(od => od.OrderDetail_Id == orderDetail_Id));
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(string order_Id)
        {
            return await Task.FromResult(_orderDetails.Where(od => od.Order_Id == order_Id).AsEnumerable());
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            orderDetail.OrderDetail_Id = (_orderDetails.Count + 1).ToString(); // Tạo ID giả lập
            _orderDetails.Add(orderDetail);
            await Task.CompletedTask;
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            var existingOrderDetail = _orderDetails.FirstOrDefault(od => od.OrderDetail_Id == orderDetail.OrderDetail_Id);
            if (existingOrderDetail != null)
            {
                existingOrderDetail.OrderDetail_Id = (orderDetail.Product_Id);
                existingOrderDetail.Quantity = orderDetail.Quantity;
                existingOrderDetail.UnitPrice = orderDetail.UnitPrice;
                existingOrderDetail.Subtotal = orderDetail.Subtotal;
                await Task.CompletedTask;
            }
        }

        public async Task DeleteOrderDetailAsync(string orderDetail_Id)
        {
            var orderDetailToRemove = _orderDetails.FirstOrDefault(od => od.OrderDetail_Id == orderDetail_Id);
            if (orderDetailToRemove != null)
            {
                _orderDetails.Remove(orderDetailToRemove);
                await Task.CompletedTask;
            }
        }

        public async Task<OrderDetail> AddProductToOrderAsync(string orderId, string productId, int quantity)
        {
            // Kiểm tra xem sản phẩm đã có trong đơn hàng chưa
            var existingOrderDetail = _orderDetails.FirstOrDefault(od => od.Order_Id == orderId && od.Product_Id == productId);

            if (existingOrderDetail != null)
            {
                // Nếu đã có, tăng số lượng
                existingOrderDetail.Quantity += quantity;
                existingOrderDetail.Subtotal = existingOrderDetail.Quantity * existingOrderDetail.UnitPrice;
                return await Task.FromResult(existingOrderDetail);
            }
            else
            {
                // Nếu chưa có, tạo mới OrderDetail
                // Giả sử bạn có một cách để lấy thông tin sản phẩm (ví dụ: ProductDao)
                var mockProduct = new Product { Product_Id = productId, ProductName = $"Product {productId}", Price = 150000 }; // Thay bằng logic thực tế
                var newOrderDetail = new OrderDetail(mockProduct, quantity) { Order_Id = orderId };
                newOrderDetail.OrderDetail_Id = (_orderDetails.Count + 1).ToString();
                _orderDetails.Add(newOrderDetail);
                return await Task.FromResult(newOrderDetail);
            }
        }

        public async Task UpdateOrderDetailQuantityAsync(string orderDetailId, int newQuantity)
        {
            var existingOrderDetail = _orderDetails.FirstOrDefault(od => od.OrderDetail_Id == orderDetailId);
            if (existingOrderDetail != null)
            {
                if (newQuantity <= 0)
                {
                    // Nếu số lượng về 0, xóa OrderDetail
                    _orderDetails.Remove(existingOrderDetail);
                }
                else
                {
                    // Cập nhật số lượng và tính lại Subtotal
                    existingOrderDetail.Quantity = newQuantity;
                    existingOrderDetail.Subtotal = existingOrderDetail.Quantity * existingOrderDetail.UnitPrice;
                }
                await Task.CompletedTask;
            }
        }
    }
}
