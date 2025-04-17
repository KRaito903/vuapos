using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DAO.Interface;

namespace vuapos.Presentation.DAO.MockData
{
    public class MockProductDao : IProductDao
    {
        public async Task<Models.Product> GetProductByIdAsync(string productId)
        {
            return await Task.FromResult(MockData.Products.FirstOrDefault(p => p.Product_Id == productId));
        }

        public async Task<IEnumerable<Models.Product>> GetAllProductsAsync()
        {
            return await Task.FromResult(MockData.Products.AsEnumerable());
        }

        public async Task AddProductAsync(Models.Product product)
        {
            product.Product_Id = MockData.Products.Count > 0 ? MockData.Products.Max(p => p.Product_Id) + 1 : "1";
            MockData.Products.Add(product);
            await Task.CompletedTask;
        }

        public async Task UpdateProductAsync(Models.Product product)
        {
            var existingProduct = MockData.Products.FirstOrDefault(p => p.Product_Id == product.Product_Id);
            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.Price = product.Price;
                existingProduct.StockQuantity = product.StockQuantity;
                existingProduct.Brand = product.Brand;
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

        public async Task<IEnumerable<Models.Product>> SearchProductsByNameAsync(string name)
        {
            return await Task.FromResult(MockData.Products.Where(p => p.ProductName.ToLower().Contains(name.ToLower())).AsEnumerable());
        }
    }
}
