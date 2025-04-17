using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DAO.Interfac;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.DAO.MockData
{
    public class MockProductVariantDao : IProductVariantDao
    {
        public async Task<IEnumerable<ProductVariant>> GetProductVariantsByProductIdAsync(string productId)
        {
            return await Task.FromResult(MockData.ProductVariants.Where(v => v.Product_Id == productId).AsEnumerable());
        }
    }
}
