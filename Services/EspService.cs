using System.Threading.Tasks;
using System.Collections.Generic;
using HttpClients;
using Models.Eskom;
using esp = Models.Esp;
using System;
using Models.Esp;

namespace Services
{


    public interface IEspService 
    {
        Task<esp.StatusObject> GetStatus();
        Task<dynamic> AreasSearch(string token, string searchText);
        Task<dynamic> AreaInformation(string token, string id);
        Task<dynamic> ApiAllowance(string token, string id);
    }

    public class EspService : IEspService
    {
        private readonly EspHttpClient _httpClient;
        private readonly ICacheService _cacheService;
        public EspService(EspHttpClient myHttpClient, ICacheService cacheService)
        {
            _httpClient = myHttpClient;
            _cacheService = cacheService;
        }
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

        public async Task<esp.StatusObject> GetStatus()
        {
            var res = _cacheService.GetCache("ESPStatus", new TimeSpan(0, 30, 0));
            if (res == null)
            {
                var cc = await _httpClient.GetStatus();
                res = System.Text.Json.JsonSerializer.Serialize<esp.StatusObject>(cc);
                _cacheService.SetCache("ESPStatus",res);

            }
            var stages = System.Text.Json.JsonSerializer.Deserialize<StatusObject>(res);
            
            return stages;
        }
    }
}