using QuickPickDBApiService.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace QuickPickDBApiService.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        public LoginService(ApiBaseUrl apiBaseUrl)
        {
            _httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(apiBaseUrl.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(apiBaseUrl.BaseUrl);
            }
        }
        public async Task<List<Models.ApiModels.Login>> GetLoginAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Models.ApiModels.Login>>("api/Login") ?? new List<Models.ApiModels.Login>();
        }
    }
}
