using HtmlAgilityPack;
using Microsoft.Net.Http.Headers;
using Models.Eskom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Enums;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Microsoft.AspNetCore.Html;

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
      resp.Content = JsonContent.Create<IEnumerable<Province>>(provinceList);
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
    public async Task<HttpResponseMessage> GetSuburbListByMunicipality(int municipalityId)
    {
      _httpClient.DefaultRequestHeaders.Add(
      HeaderNames.Accept, "application/json");
      return await _httpClient.GetAsync("GetSurburbData/?pageSize=9200&pageNum=1&id=" + municipalityId);
    }
    public async Task<HttpResponseMessage> IsEskomClient(int suburbId, string provinceName, int x, int total)
    {
      _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
      var res = await _httpClient.GetAsync(string.Format("GetScheduleM/{0}/{1}/{2}/{3}", suburbId, 4, provinceName, total)).Result.Content.ReadAsStringAsync();
      var b = res.Contains("We regret that we could not");
      var resp = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
      if (!b)
      {
        resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
      }
      return await Task.FromResult(resp);

    }

    public async Task<HttpResponseMessage> GetSchedule(int blockId, int stage)
    {
      _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
      var res = await _httpClient.GetAsync(string.Format("GetScheduleM/{0}/{1}/_/1", blockId, stage));
      var resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
      if ((await res.Content.ReadAsStringAsync()).Contains("We regret that we could not"))
      {
        resp = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
      }
      else
      {
        resp.Content = res.Content;
      }
      return await Task.FromResult(resp);
    }
    public async Task<HttpResponseMessage> GetStatus()
    {
      var htmlContent = await _httpClient.GetAsync("GetStatus").Result.Content.ReadAsStringAsync();
      // var htmlContent = "<html><head><meta name = \"color-scheme\" content = \"light dark\"></head><body><pre style = \"word-wrap: break-word; white-space: pre-wrap;\" > 5 </pre></body></html>";
      // Find the index of the closing "</pre>" tag
      int endIndex = htmlContent.IndexOf("</pre>");

      // Extract the text between the opening and closing tags
      string res = htmlContent.Substring((endIndex - 2), 1).Trim();

     var resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
      resp.Content = new StringContent(res);
      return await Task.FromResult(resp);
    }
  }
}
