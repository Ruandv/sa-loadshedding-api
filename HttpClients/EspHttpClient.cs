using Enums;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClients
{
  public class EspHttpClient
    {
        private readonly HttpClient _httpClient;
        public EspHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable(EnvironmentVariableNames.ESP_BASE_URL.ToString()));
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.UserAgent, "HttpRequestsSample");
        }

        public async Task<dynamic> GetStatus(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("token", token);
            var c = await _httpClient.GetAsync("status").Result.Content.ReadAsStringAsync();
            dynamic dynamicObje3ct = System.Text.Json.JsonSerializer.Deserialize<dynamic>(c);
            return dynamicObje3ct;
        }
        public async Task<dynamic> AreasSearch(string token, string searchText)
        {
            _httpClient.DefaultRequestHeaders.Add("token", token);
            var data = await _httpClient.GetAsync("areas_search?text=" + searchText).Result.Content.ReadAsStringAsync();

            dynamic dynamicObje3ct = System.Text.Json.JsonSerializer.Deserialize<dynamic>(data);
            return dynamicObje3ct;
        }

        public async Task<dynamic> AreaInformation(string token, string id)
        {

            _httpClient.DefaultRequestHeaders.Add("token", token);
            var data = await _httpClient.GetAsync("area?id=" + id + "").Result.Content.ReadAsStringAsync();
            dynamic dynamicObje3ct = System.Text.Json.JsonSerializer.Deserialize<dynamic>(data);
            return dynamicObje3ct;
        }

        public async Task<dynamic> ApiAllowance(string token, string id)
        {
            _httpClient.DefaultRequestHeaders.Add("token", token);
            var res = await _httpClient.GetAsync("api_allowance").Result.Content.ReadAsStringAsync();
            dynamic dynamicObje3ct = System.Text.Json.JsonSerializer.Deserialize<dynamic>(res);
            return dynamicObje3ct;
        }

    }

}
