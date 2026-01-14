using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace QuickPickDBApiService.Services
{
    public class ItemService
    {
        private readonly HttpClient _httpClient;
        public ItemService(Models.ApiBaseUrl url)
        {
            _httpClient = new HttpClient();
            if(!string.IsNullOrEmpty(url.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(url.BaseUrl);
            }
        }
        public async Task<List<Models.ApiModels.Item>> GetItemsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Models.ApiModels.Item>>("api/Item") ?? new List<Models.ApiModels.Item>();
            }
            catch (Exception)
            {
                return new List<Models.ApiModels.Item>();
            }
        }
        public async Task<Models.ApiModels.Item?> CreateItemAsync(Models.ApiModels.Item item)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Item", item);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Models.ApiModels.Item>();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> UpdateItemAsync(Models.ApiModels.Item item)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/Item", item);
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
                var response = await _httpClient.DeleteAsync($"api/Item/{id}");
                return response.IsSuccessStatusCode;

            }
            catch
            {
                return false;
            }
        }
    }
}
