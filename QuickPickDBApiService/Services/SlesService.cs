using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace QuickPickDBApiService.Services
{
    public class SlesService
    {
        private readonly HttpClient _httpClient;
        public SlesService( Models.ApiBaseUrl url)
        {
            _httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(url.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(url.BaseUrl);
            }
        }
        public async Task<List<Models.ApiModels.Sale>> GetSlesAsync()
        {
          return await _httpClient.GetFromJsonAsync<List<Models.ApiModels.Sale>>("api/Transaction") ?? new List<Models.ApiModels.Sale>();
        }
        public async Task<Models.ApiModels.Sale?> CreateSlesAsync(Models.ApiModels.Sale sles)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Transaction", sles);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Models.ApiModels.Sale>();
            }
            return null;
        }
        public async Task<bool> UpdateSlesAsync(Models.ApiModels.Sale sles)
        {
            var response = await _httpClient.PutAsJsonAsync("api/Transaction", sles);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteSlesAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Transaction/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
