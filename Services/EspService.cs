using System.Threading.Tasks;
using System.Collections.Generic;
using HttpClients;
using Models.Eskom;
using System;

namespace Services
{


    public interface IEspService : INationalService
    {
        Task<dynamic> AreasSearch(string token, string searchText);
        Task<dynamic> AreaInformation(string token, string id);
        Task<dynamic> ApiAllowance(string token, string id);
    }

    public class EspService : IEspService
    {
        private readonly EspHttpClient _httpClient;

        public Task<dynamic> ApiAllowance(string token, string id)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> AreaInformation(string token, string id)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> AreasSearch(string token, string searchText)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Province>> GetProvinces()
        {
            throw new NotImplementedException();
        }

        public async Task<StatusObject> GetStatus<StatusObject>(string token)
        {
            var str = @"{
    ""status"": {
        ""capetown"": {
            ""name"": ""Cape Town"",
            ""next_stages"": [
                {
                    ""stage"": ""1"",
                    ""stage_start_timestamp"": ""2022-08-08T17:00:00+02:00""
                },
                {
                    ""stage"": ""0"",
                    ""stage_start_timestamp"": ""2022-08-08T22:00:00+02:00""
                }
            ],
            ""stage"": ""0"",
            ""stage_updated"": ""2022-08-08T00:08:16.837063+02:00""
        },
        ""eskom"": {
            ""name"": ""National"",
            ""next_stages"": [
                {
                    ""stage"": ""2"",
                    ""stage_start_timestamp"": ""2022-08-08T16:00:00+02:00""
                },
                {
                    ""stage"": ""0"",
                    ""stage_start_timestamp"": ""2022-08-09T00:00:00+02:00""
                }
            ],
            ""stage"": ""6"",
            ""stage_updated"": ""2022-08-08T16:12:53.725852+02:00""
        }
    }
}";
            var res = str;
            //await _httpClient.GetStatus(token).Result.Content.ReadAsStringAsync();
            var stages = System.Text.Json.JsonSerializer.Deserialize<StatusObject>(res);
            return stages;
            //_cacheService.SetCache("GetStatus", System.Text.Json.JsonSerializer.Serialize<int>(intValue));
            //return intValue;
            //return res.StatusCode;
        }
    }
}