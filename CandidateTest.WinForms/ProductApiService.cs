using CandidateTest.WinForms.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTest.WinForms
{
    public class ProductApiService : IDisposable
    {
        private readonly HttpClient _httpClient;
        public void Dispose() => _httpClient.Dispose();
        public ProductApiService(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }
        public async Task<List<ProductDto>> GetAllAsync()
            => await _httpClient.GetFromJsonAsync<List<ProductDto>>("api/products") ?? new();
        public async Task<ProductDto?> GetByIdAsync(int id)
            => await _httpClient.GetFromJsonAsync<ProductDto>($"api/products/{id}");
        public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
        {
            var resp = await _httpClient.PostAsJsonAsync("api/products", dto);
            resp.EnsureSuccessStatusCode();
            return (await resp.Content.ReadFromJsonAsync<ProductDto>())!;
        }

        public async Task UpdateAsync(int id, ProductUpdateDto dto)
        {
            var resp = await _httpClient.PutAsJsonAsync($"api/products/{id}", dto);
            resp.EnsureSuccessStatusCode();
        }
        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<(List<ProductDto> Items, int Total)> GetPagedAsync(
        int page, int pageSize,
        string? name = null, decimal? minPrice = null, decimal? maxPrice = null,
        IEnumerable<int>? categoryIds = null)
            {
                var qs = new List<string>
                {
                    $"page={page}",
                    $"pageSize={pageSize}"
                };
                if (!string.IsNullOrWhiteSpace(name)) qs.Add($"name={Uri.EscapeDataString(name)}");
                if (minPrice.HasValue) qs.Add($"minPrice={minPrice.Value}");
                if (maxPrice.HasValue) qs.Add($"maxPrice={maxPrice.Value}");
                if (categoryIds is { } ids && ids.Any()) qs.Add($"categories={string.Join(",", ids)}");

                var url = "api/products" + "?" + string.Join("&", qs);
                using var resp = await _httpClient.GetAsync(url);
                resp.EnsureSuccessStatusCode();

                var totalStr = resp.Headers.TryGetValues("X-Total-Count", out var values) ? values.FirstOrDefault() : "0";
                var total = int.TryParse(totalStr, out var t) ? t : 0;

                var items = await resp.Content.ReadFromJsonAsync<List<ProductDto>>() ?? new List<ProductDto>();
                return (items, total);
            }

    }
}
