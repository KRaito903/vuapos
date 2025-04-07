using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Views.Category;

namespace vuapos.Presentation.Services
{
    public interface ICategoryService
    {
        Task<List<Category>?> GetAllCategoriesAsync();
        Task<Category?> AddCategoryAsync(string name);
        Task<Category?> UpdateCategoryAsync(string customer_id, string name);
        Task<Category?> DeleteCategoryAsync(string customer_id);
    }
    public class CategoryService : ApiService, ICategoryService
    {
        public CategoryService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjBjYjU1MmIwLTQxNTItNDA3NC1hYmVmLTFiMmQwZTU2ZmI0NCIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ0MDM5NjY4LCJleHAiOjE3NDQ2NDQ0Njh9.--bN49s9dQlxv_jLzvPDzwc_UCHeAVscm5MvCy-gXu8";
        }

        public async Task<List<Category>?> GetAllCategoriesAsync()
        {
            return await SendRequestAsync<List<Category>>(HttpMethod.Get, "category");
        }

        public async Task<Category?> AddCategoryAsync(string name)
        {
            var categoryData = new { name };
            return await SendRequestAsync<Category>(HttpMethod.Post, "category", categoryData);
        }

        public async Task<Category?> UpdateCategoryAsync(string customer_id, string name)
        {
            var categoryData = new { name };
            return await SendRequestAsync<Category>(HttpMethod.Patch, $"category/{customer_id}", categoryData);
        }

        public async Task<Category?> DeleteCategoryAsync(string customer_id)
        {
            return await SendRequestAsync<Category>(HttpMethod.Delete, $"category/{customer_id}");
        }
    }
}
