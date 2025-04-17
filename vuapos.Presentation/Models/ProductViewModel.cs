using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.DTO.Product;
using vuapos.Presentation.Services;
using vuapos.Presentation.Views.Category;
using vuapos.Presentation.Views.Product;
using Windows.Storage;

namespace vuapos.Presentation.Models
{

    public class ProductViewModel
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly ProductService _productService;

        public ObservableCollection<Views.Product.Product> Products { get; set; } = new();
        public ProductViewModel()
        {
            _productService = App.Services.GetRequiredService<ProductService>();
            _cloudinaryService = App.Services.GetRequiredService<CloudinaryService>();
        }

        public async Task LoadProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products != null)
            {
                Products.Clear();
                foreach (var product in products)
                    Products.Add(product);
            }
        }

        public async Task AddProductAsync(string productName, string categoryId, decimal price, decimal costPrice, int stockQuantity, StorageFile imageFile = null)
        {
            try
            {
                string imageUrl = string.Empty;
                if (imageFile != null)
                {
                    imageUrl = await _cloudinaryService.UploadImageAsync(imageFile);
                    Debug.WriteLine($"Uploaded image URL: {imageUrl}");
                }

                var productCreateDTO = new ProductCreateDTO
                {
                    product_name = productName,
                    category_id = categoryId,
                    price = price,
                    cost_price = costPrice,
                    stock_quantity = stockQuantity,
                    discount = 0,
                    image_path = imageUrl
                };

                var product = await _productService.AddProductAsync(productCreateDTO);
                Debug.WriteLine($"Product: {product}");
                if (product != null)
                {
                    Products.Add(product);

                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating product.", ex);
            }
        }
        public async Task<bool> DeleteProductAsync(string productId)
        {
            var success = await _productService.DeleteProductAsync(productId);
            if (success)
            {
                var product = Products.FirstOrDefault(p => p.Product_Id == productId);
                if (product != null)
                {
                    Products.Remove(product);
                }
            }
            return success;
        }
    }
}
