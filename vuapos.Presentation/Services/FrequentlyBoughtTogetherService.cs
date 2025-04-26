using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Views.FrequentlyBoughtTogether;

namespace vuapos.Presentation.Services
{
    public class FrequentlyBoughtTogetherService: ApiService
    {
        public FrequentlyBoughtTogetherService(HttpClient httpClient) : base(httpClient)
        {
            base.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdGFmZl9pZCI6IjBjYjU1MmIwLTQxNTItNDA3NC1hYmVmLTFiMmQwZTU2ZmI0NCIsInJvbGUiOiJNQU5BR0VSIiwiaWF0IjoxNzQ1NDkyNTk4LCJleHAiOjE3NDYwOTczOTh9._dhH4UZRNzp70jKeWus61XnMZ7Nt6lZWUwr-e2lNMds";
        }
        public async Task<FrequentlyBoughtTogether?> GetFrequentlyBoughtTogetherAsync()
        {
            Debug.WriteLine("Sending request to /analyze");
            return await SendRequestAsync<FrequentlyBoughtTogether>(HttpMethod.Get, "analyze");
        }
    }
}
