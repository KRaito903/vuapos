using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.DAO.Interface
{
    public interface IProductDao
    {
        Task<Models.Product> GetProductByIdAsync(string productId);
        Task<IEnumerable<Models.Product>> GetAllProductsAsync();
        Task AddProductAsync(Models.Product product);
        Task UpdateProductAsync(Models.Product product);
        Task DeleteProductAsync(string productId);
        Task<IEnumerable<Models.Product>> SearchProductsByNameAsync(string name);
    }
}
