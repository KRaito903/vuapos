using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Product;
using vuapos.Presentation.Views.Category;
using vuapos.Presentation.Views.Product;

namespace vuapos.Presentation.Services
{
    public class ProductService: ApiService
    {
        CloudinaryService _cloudinaryService;
        public ProductService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjBjYjU1MmIwLTQxNTItNDA3NC1hYmVmLTFiMmQwZTU2ZmI0NCIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ1NDkyNTk4LCJleHAiOjE3NDYwOTczOTh9._dhH4UZRNzp70jKeWus61XnMZ7Nt6lZWUwr-e2lNMds";
            _cloudinaryService = new CloudinaryService();

        }
        public async Task<Product?> GetProductAsync(string productId)
        {
            return await SendRequestAsync<Product>(HttpMethod.Get, $"product/{productId}");
        }
        //public async Task<List<Product>?> GetAllProductsAsync()
        //{
        //    return await SendRequestAsync<List<Product>>(HttpMethod.Get, "product");
        //}   
        public async Task<PageProductResponse<Product>?> GetAllProductsAsync(int page = 1)
        {
            return await SendRequestAsync<PageProductResponse<Product>>(HttpMethod.Get, $"product?page={page}");
        }


        public async Task<Product?> AddProductAsync(ProductCreateDTO productCreateDTO)
        {
            Debug.WriteLine($"ProductCreateDTO: {productCreateDTO}");
            return await SendRequestAsync<Product>(HttpMethod.Post, "product", productCreateDTO);
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var product = await GetProductAsync(productId);
            if (product == null)
            {
                Debug.WriteLine($"Product with ID {productId} not found.");
                return false;
            }
            var publicId = ExtractPublicIdFromImagePath(product.Image_Path);
            Debug.WriteLine($"Public ID: {publicId}");
            if (string.IsNullOrEmpty(publicId))
            {
                Debug.WriteLine("Failed to extract public_id from Image_Path.");
                return false;
            }

            var deleteImageSuccess = await _cloudinaryService.DeleteImageAsync(publicId);
            if (deleteImageSuccess)
            {
                Debug.WriteLine($"Image {publicId} deleted from Cloudinary.");
            }
            else
            {
                return false;
            }

            var deleteProductSuccess = await SendRequestAsync<Product>(HttpMethod.Delete, $"product/{productId}");
            if (deleteProductSuccess != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string ExtractPublicIdFromImagePath(string imagePath)
        {
                var uri = new Uri(imagePath);
                var segments = uri.AbsolutePath.Split('/');

                int uploadIndex = Array.IndexOf(segments, "image");
                if (uploadIndex == -1 || uploadIndex + 2 >= segments.Length)
                {
                    throw new ArgumentException("Invalid Cloudinary URL format.");
                }

                var publicIdSegments = segments.Skip(uploadIndex + 3);
                var publicIdWithExtension = string.Join("/", publicIdSegments);
                var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);

                if (publicIdSegments.Count() > 1)
                {
                    publicId = string.Join("/", publicIdSegments.Take(publicIdSegments.Count() - 1)) + "/" + publicId;
                }

                return publicId;
            
        }

        public async Task<Product?> UpdateProductAsync(string productId, ProductUpdateDTO productUpdateDTO)
        {
            return await SendRequestAsync<Product>(HttpMethod.Patch, $"product/{productId}", productUpdateDTO);
        }

    }
}
