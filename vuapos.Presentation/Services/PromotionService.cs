using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
//using System.Text.Json;
using Newtonsoft.Json;

using System.Text.Json.Serialization;
using System.Threading.Tasks;
using vuapos.Presentation.DTO.Promotion;
using vuapos.Presentation.Views.Category;
using vuapos.Presentation.Views.Promotion;

namespace vuapos.Presentation.Services
{
    public class PromotionService: ApiService
    {
        public PromotionService(HttpClient httpClient): base(httpClient)
        {
            base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjBjYjU1MmIwLTQxNTItNDA3NC1hYmVmLTFiMmQwZTU2ZmI0NCIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ1NDkyNTk4LCJleHAiOjE3NDYwOTczOTh9._dhH4UZRNzp70jKeWus61XnMZ7Nt6lZWUwr-e2lNMds";
        }
        public async Task<PagePromotionResponse<Promotion>?> GetPaginationPromotionAsync(int page = 1)
        {
            return await SendRequestAsync<PagePromotionResponse<Promotion>>(HttpMethod.Get, $"promotions?page={page}");

        }
        public async Task<Promotion?> AddPromotionAsync(PromotionCreateDTO promotionCreateDTO)
        {
            Debug.WriteLine(promotionCreateDTO.name);

            Debug.WriteLine("duweuhfwhefoihweofhoewhf");


            return await SendRequestAsync<Promotion>(HttpMethod.Post, "promotions", promotionCreateDTO);
        }

        public async Task<Promotion?> UpdatePromotionAsync(PromotionUpdateDTO promotion, string id)
        {
            return await SendRequestAsync<Promotion>(HttpMethod.Patch, $"promotions/{id}", promotion);
        }
        public async Task<Promotion?> DeletePromotionAsync(string id)
        {
            return await SendRequestAsync<Promotion>(HttpMethod.Delete, $"promotions/{id}");
        }

    }
}
