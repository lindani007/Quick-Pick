using QuickPickDBApiService.Models;
using QuickPickDBApiService.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace QuickPickDBApiService.Services
{
    public class AisleService
    {
        private readonly HttpClient _httpClient;
        public AisleService( ApiBaseUrl url)
        {
            _httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(url.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(url.BaseUrl);
            }
        }
        public async Task<List<Aisle>> GetAislesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Aisle>>("api/Aisle") ?? new List<Aisle>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching aisles: {ex.Message}");
                return new List<Aisle>();
            }
        }
        public async Task<Aisle?> CreateAisleAsync(Aisle aisle)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Aisle", aisle);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Aisle>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> UpdateAisleAsync(Aisle aisle)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/Aisle", aisle);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteAisleAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Aisle/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
