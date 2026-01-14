using QuickPickDBApiService.Models;
using QuickPickDBApiService.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace QuickPickDBApiService.Services
{
    public class StockService
    {
        HttpClient _httpclient;
        public StockService(ApiBaseUrl apiBaseUrl)
        {
            _httpclient = new HttpClient();
            if(!string.IsNullOrEmpty(apiBaseUrl.BaseUrl))
            {
                _httpclient.BaseAddress = new Uri(apiBaseUrl.BaseUrl);
            }
        }
        public async Task<List<Stock>> LoadStocks()
        {
            try
            {
                return await _httpclient.GetFromJsonAsync<List<Stock>>("api/Stock") ?? new List<Stock>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return new List<Stock>();
            }
        }
        public async Task<Stock?> AddStock(Stock stock)
        {
            try
            {
                var response = await _httpclient.PostAsJsonAsync<Stock>("api/Stock", stock);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Stock>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public async Task<Stock> UpdateStock(Stock stock)
        {
            try
            {
                var response = await _httpclient.PutAsJsonAsync<Stock>("api/Stock", stock);
                return stock;
            }
            catch
            {
                return stock;
            }
        }
        public async Task<bool> DeleteStock(int id)
        {
            try
            {
                var response = await _httpclient.DeleteAsync($"api/Stock/{id}");
                return response.IsSuccessStatusCode;

            }
            catch
            {
                return false;
            }
        }
    }
}
