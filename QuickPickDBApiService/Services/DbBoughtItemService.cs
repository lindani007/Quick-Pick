using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace QuickPickDBApiService.Services
{
    public class DbBoughtItemService
    {
        private readonly HttpClient _httpClient;
        public DbBoughtItemService(Models.ApiBaseUrl url)
        {
            _httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(url.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(url.BaseUrl);
            }
        }
        public async Task<List<Models.ApiModels.BoughtItem>> GetItemsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Models.ApiModels.BoughtItem>>("api/BoughtItem") ?? new List<Models.ApiModels.BoughtItem>();
            }
            catch
            {
                return new List<Models.ApiModels.BoughtItem>();
            }
        }
        public async Task<List<Models.ApiModels.BoughtItem>> CreateItemAsync(List<Models.ApiModels.BoughtItem> items)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/BoughtItem", items);
                return await response.Content.ReadFromJsonAsync<List<Models.ApiModels.BoughtItem>>() ?? new List<Models.ApiModels.BoughtItem>();
            }
            catch
            {
                return new List<Models.ApiModels.BoughtItem>();
            }
        }
        public async Task<bool> UpdateItemAsync(List<Models.ApiModels.BoughtItem> items)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/BoughtItem", items);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/BoughtItem/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
