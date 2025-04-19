using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DAO.Interface;
using vuapos.Presentation.Views.Product;

namespace vuapos.Presentation.DAO.MockData
{
    public class MockProductDao : IProductDao
    {
        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await Task.FromResult(MockData.Products.FirstOrDefault(p => p.Product_Id == productId));
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await Task.FromResult(MockData.Products.AsEnumerable());
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = MockData.Products.FirstOrDefault(p => p.Product_Id == product.Product_Id);
            if (existingProduct != null)
            {
                existingProduct.Product_Name = product.Product_Name;
                existingProduct.Price = product.Price;
                existingProduct.Stock_Quantity = product.Stock_Quantity;
                await Task.CompletedTask;
            }
        }

        public async Task DeleteProductAsync(string productId)
        {
            var productToRemove = MockData.Products.FirstOrDefault(p => p.Product_Id == productId);
            if (productToRemove != null)
            {
                MockData.Products.Remove(productToRemove);
                await Task.CompletedTask;
            }
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameAsync(string name)
        {
            return await Task.FromResult(MockData.Products.Where(p => p.Product_Name.ToLower().Contains(name.ToLower())).AsEnumerable());
        }
    }
}
