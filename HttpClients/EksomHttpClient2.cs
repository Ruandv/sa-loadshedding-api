using EskomCalendarApi.Enums;
using Microsoft.Net.Http.Headers;
using Models.Eskom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HttpClients
{
    public class EskomHttpClient2
    {
        private readonly HttpClient _httpClient;
        private List<Province> provinceList = new List<Province>();
        private Dictionary<int, IEnumerable<Municipality>> provinceMunicipalityDictionary = new Dictionary<int, IEnumerable<Municipality>>();


        public EskomHttpClient2(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable(EnvironmentVariableNames.ESKOM_SITE_BASE_URL.ToString()));
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Mozilla/5.0 (X11; Linux x86_64; rv:69.0) Gecko/20100101 Firefox/69.0");

            provinceList.Add(new Province() { ProvinceId = 1, ProvinceName = "Eastern Cape" });
            provinceList.Add(new Province() { ProvinceId = 2, ProvinceName = "Free State" });
            provinceList.Add(new Province() { ProvinceId = 3, ProvinceName = "Gauteng" });
            provinceList.Add(new Province() { ProvinceId = 4, ProvinceName = "KwaZulu-Natal" });
            provinceList.Add(new Province() { ProvinceId = 5, ProvinceName = "Limpopo" });
            provinceList.Add(new Province() { ProvinceId = 6, ProvinceName = "Mpumalanga" });
            provinceList.Add(new Province() { ProvinceId = 7, ProvinceName = "North West" });
            provinceList.Add(new Province() { ProvinceId = 8, ProvinceName = "Northern Cape" });
            provinceList.Add(new Province() { ProvinceId = 9, ProvinceName = "Western Cape" });
        }
        public async Task<HttpResponseMessage> GetProvinceList()
        {
            // For now the API only support Gauteng
            var resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            resp.Content = System.Net.Http.Json.JsonContent.Create<IEnumerable<Province>>(provinceList);
            return await Task.FromResult(resp);
        }
        public async Task<HttpResponseMessage> GetProvinceByName(string provinceName)
        {
            var resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            resp.Content = System.Net.Http.Json.JsonContent.Create(provinceList.ToList().FirstOrDefault(x => x.ProvinceName == provinceName));
            return await Task.FromResult(resp);
        }

        public async Task<HttpResponseMessage> GetMunicipalityByName(int provinceId, string municipalityName)
        {
            var list = provinceMunicipalityDictionary.FirstOrDefault(x => x.Key == provinceId);
            if (list.Value == null)
            {
                // go fetch and add it to the list.
                var r = await this.GetMunicipalityList(provinceId).Result.Content.ReadFromJsonAsync<IEnumerable<Municipality>>();
                list = new KeyValuePair<int, IEnumerable<Municipality>>(provinceId, r);
                provinceMunicipalityDictionary.Add(provinceId, r);
            }
            var res = list.Value.ToList().FirstOrDefault(x => x.MunicipalityName == municipalityName);
            var resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            resp.Content = JsonContent.Create(res);
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

        //public async Task<HttpResponseMessage> SearchSuburb(string searchTerm, int municipalityId)
        //{
        //    _httpClient.DefaultRequestHeaders.Add(
        //    HeaderNames.Accept, "application/json");
        //    return await _httpClient.GetAsync("/GetSurburbData/?pageSize=100&pageNum=1&searchTerm=" + searchTerm + "&id=" + municipalityId);
        //}

    }
}
