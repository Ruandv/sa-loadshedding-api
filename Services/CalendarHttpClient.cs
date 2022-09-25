using EskomCalendarApi.Enums;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EskomCalendarApi.Services
{
    public class CalendarHttpClient
    {
        private readonly HttpClient _httpClient;
        public CalendarHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable(EnvironmentVariableNames.ESKOM_CALENDAR_BASE_URL.ToString()));
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.UserAgent, "HttpRequestsSample");
        }

        public async Task<HttpResponseMessage> GetCalendarByName(string calendarName)
        {
            return await _httpClient.GetAsync(calendarName);
        }

        public async Task<Stream> GetMachineFriendlyFile()
        {
            //machine_friendly.csv
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/csv");
            return await _httpClient.GetStreamAsync("machine_friendly.csv");
        }

        public async Task<HttpResponseMessage> GetAssetData()
        {
            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            return await _httpClient.GetAsync("https://api.github.com/repos/beyarkay/eskom-calendar/releases/latest");
        }

    }

}
