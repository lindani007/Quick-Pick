using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace QuickPickDBApiService.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        public OrderService( Models.ApiBaseUrl url)
        {
            _httpClient = new HttpClient();
            if(!string.IsNullOrEmpty(url.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(url.BaseUrl);
            }
        }
        public async Task<List<Models.ApiModels.Order>> GetOrdersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Models.ApiModels.Order>>("api/Order") ?? new List<Models.ApiModels.Order>();
            }
            catch
            {
                               return new List<Models.ApiModels.Order>();
            }
        }
        public async Task<Models.ApiModels.Order?> CreateOrderAsync(Models.ApiModels.Order order)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Order", order);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Models.ApiModels.Order>();
                }
                return null;
            }
            catch
            { return null; }
        }
        public async Task<bool> UpdateOrderAsync(Models.ApiModels.Order order)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/Order", order);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteOrderAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Order/{id}");
                return response.IsSuccessStatusCode;
            }
            catch 
            { return false; }
        }
    }
}
