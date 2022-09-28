using EskomCalendarApi.Enums;
using EskomCalendarApi.Models.Calendar;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EskomCalendarApi.Services
{
    public class EskomHttpClient
    {
        private readonly HttpClient _httpClient;
        public EskomHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable(EnvironmentVariableNames.ESKOM_SITE_BASE_URL.ToString()));
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.UserAgent, "HttpRequestsSample");
        }

        public async Task<HttpResponseMessage> GetProvinceList()
        {
            // For now the API only support Gauteng
            List<Province> provinceList = new List<Province>();
            provinceList.Add(new Province() { ProvinceId = 1, ProvinceName = "Eastern Cape" });
            provinceList.Add(new Province() { ProvinceId = 2, ProvinceName = "Free State" });
            provinceList.Add(new Province() { ProvinceId = 3, ProvinceName = "Gauteng" });
            provinceList.Add(new Province() { ProvinceId = 4, ProvinceName = "KwaZulu-Natal" });
            provinceList.Add(new Province() { ProvinceId = 5, ProvinceName = "Limpopo" });
            provinceList.Add(new Province() { ProvinceId = 6, ProvinceName = "Mpumalanga" });
            provinceList.Add(new Province() { ProvinceId = 7, ProvinceName = "North West" });
            provinceList.Add(new Province() { ProvinceId = 8, ProvinceName = "Northern Cape" });
            provinceList.Add(new Province() { ProvinceId = 9, ProvinceName = "Western Cape" });
            var resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            resp.Content = System.Net.Http.Json.JsonContent.Create<IEnumerable<Province>>(provinceList);
            return await Task.FromResult(resp);
        }

        public async Task<HttpResponseMessage> GetMunicipalityList(int provinceId)
        {
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/json");
            return await _httpClient.GetAsync("GetMunicipalities?id=" + provinceId);
        }
        public async Task<HttpResponseMessage> FindSuburb(string suburbName)
        {
            _httpClient.DefaultRequestHeaders.Add(
               HeaderNames.Accept, "application/json");
            return await _httpClient.GetAsync("FindSuburbs?searchText=" + suburbName + "&maxResults=300");
        }
    }
}
