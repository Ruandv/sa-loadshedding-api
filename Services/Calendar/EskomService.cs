using System.Threading.Tasks;
using System.Collections.Generic;
using EskomCalendarApi.Models.Calendar;
using System.Net.Http.Json;
using System.Linq;
using System.IO;
using System;
using EskomCalendarApi.Models.Eskom;
using System.Text.Json;

namespace EskomCalendarApi.Services.Calendar
{
    public interface IEskomService
    {
        Task<IEnumerable<Province>> GetProvinces();
        Task<IEnumerable<Municipality>> GetMunicipalities(int provinceId);
        Task<IEnumerable<SuburbData>> GetSuburbsByMunicipality(int municipalityId, int? blockId = null);
        Task<IEnumerable<SuburbSearch>> FindSuburb(string suburbName);
        Task<IEnumerable<ScheduleDto>> GetSchedule(int municipalityId, int blockId, int days, int stage);
    }

    public class EskomService : IEskomService
    {
        private readonly EskomHttpClient _httpClient;

        public EskomService(EskomHttpClient myHttpClient)
        {
            _httpClient = myHttpClient;
        }

        public async Task<IEnumerable<Province>> GetProvinces()
        {
            var data = await _httpClient.GetProvinceList().Result.Content.ReadFromJsonAsync<IEnumerable<Province>>();
            return await Task.FromResult(data);
        }

        public async Task<IEnumerable<Municipality>> GetMunicipalities(int provinceId)
        {
            // For now we only support COJ and Tshwane
            var data = await _httpClient.GetMunicipalityList(provinceId).Result.Content.ReadFromJsonAsync<IEnumerable<Municipality>>();
            data = data.ToList().Where(x => new int[] { 166, 167,168}.Contains(x.MunicipalityId));
            return await Task.FromResult(data);
        }
        public async Task<IEnumerable<ScheduleDto>> GetSchedule(int municipalityId, int blockId, int days, int stage)
        {
            // For now we only support COJ
            var dt = await _httpClient.GetSchedule(blockId, municipalityId, days, stage).Result.Content.ReadAsStringAsync();
            return (await Task.FromResult(JsonSerializer.Deserialize<List<ScheduleDto>>(dt)));
        }

        public async Task<IEnumerable<SuburbData>> GetSuburbsByMunicipality(int municipalityId, int? blockId)
        {

            //read the file from JSONData/Municipality_[MunicipalityId].json
            using (var stream = new StreamReader("./JSONData/Municipality_" + municipalityId + ".json"))
            {
                var s = JsonSerializer.Deserialize<List<SuburbData>>(stream.ReadToEnd());
                if (blockId.HasValue)
                {
                    return await Task.FromResult(s.ToList().Where(x => int.Parse(x.BlockId) == blockId));
                }
                return await Task.FromResult(s);
            }

        }
        public async Task<IEnumerable<SuburbSearch>> FindSuburb(string suburbName)
        {
            var data = await _httpClient.FindSuburb(suburbName).Result.Content.ReadFromJsonAsync<IEnumerable<SuburbSearch>>();

            return await Task.FromResult(data);// data;
        }
    }
}