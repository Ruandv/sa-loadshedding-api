using System.Threading.Tasks;
using System.Collections.Generic;
using EskomCalendarApi.Models.Eskom;
using HttpClients;
using System.Net.Http.Json;
using Models.Eskom;
using Models.Eskom.ResponseDto;
using System.Linq;
using AutoMapper;
using System.Text.RegularExpressions;

namespace Services.Eskom
{
    public interface IMunicipalService
    {
        Task<IEnumerable<SuburbData>> GetSuburbs(int? blockId = null);
        Task<IEnumerable<SuburbSearchResponseDto>> FindSuburb(string suburbName);
        Task<IEnumerable<ScheduleDto>> GetSchedule(int municipalityId, int blockId, int days, int stage);
    }

    public interface IProvincialService
    {
        Task<IEnumerable<Municipality>> GetMunicipalities(int provinceId);
    }
    public interface INationalService
    {
        Task<IEnumerable<Province>> GetProvinces();
    }

    public interface IEskomService : INationalService, IProvincialService, IMunicipalService
    {


    }

    public class EskomService : IEskomService
    {
        private readonly EskomHttpClient2 _httpClient;
        private readonly IMapper _mapper;

        public EskomService(EskomHttpClient2 myHttpClient, IMapper mapper)
        {
            _httpClient = myHttpClient;
            _mapper = mapper;
        }

        //public async Task<IEnumerable<Province>> GetProvinces()
        //{
        //    var data = await _httpClient.GetProvinceList().Result.Content.ReadFromJsonAsync<IEnumerable<Province>>();
        //    return await Task.FromResult(data);
        //}

        //public async Task<IEnumerable<Municipality>> GetMunicipalities(int provinceId)
        //{
        //    // For now we only support COJ and Tshwane
        //    var data = await _httpClient.GetMunicipalityList(provinceId).Result.Content.ReadFromJsonAsync<IEnumerable<Municipality>>();
        //    data = data.ToList();
        //    return await Task.FromResult(data);
        //}
        //public async Task<IEnumerable<ScheduleDto>> GetSchedule(int municipalityId, int blockId, int days, int stage)
        //{
        //    // For now we only support COJ
        //    var dt = await _httpClient.GetSchedule(blockId, municipalityId, days, stage).Result.Content.ReadAsStringAsync();
        //    return await Task.FromResult(JsonSerializer.Deserialize<List<ScheduleDto>>(dt));
        //}

        //public async Task<IEnumerable<SuburbData>> GetSuburbsByMunicipality(int municipalityId, int? blockId)
        //{
        //    //read the file from JSONData/Municipality_[MunicipalityId].json
        //    using (var stream = new StreamReader("./JSONData/Municipality_" + municipalityId + ".json"))
        //    {
        //        var s = JsonSerializer.Deserialize<List<SuburbData>>(stream.ReadToEnd());
        //        if (blockId.HasValue)
        //        {
        //            return await Task.FromResult(s.ToList().Where(x => int.Parse(x.BlockId) == blockId).OrderBy(x => x.SubName));
        //        }
        //        return await Task.FromResult(s.OrderBy(x => x.SubName));
        //    }
        //}
        //public async Task<IEnumerable<SuburbSearch>> FindSuburb(string suburbName)
        //{
        //    var data = await _httpClient.FindSuburb(suburbName).Result.Content.ReadFromJsonAsync<IEnumerable<SuburbSearch>>();

        //    return await Task.FromResult(data);// data;
        //}

        public async Task<IEnumerable<Province>> GetProvinces()
        {
            return await _httpClient.GetProvinceList().Result.Content.ReadFromJsonAsync<IEnumerable<Province>>();
        }
        public async Task<IEnumerable<Municipality>> GetMunicipalities(int provinceId)
        {
            return await _httpClient.GetMunicipalityList(provinceId).Result.Content.ReadFromJsonAsync<IEnumerable<Municipality>>();
        }

        public async Task<IEnumerable<SuburbSearchResponseDto>> FindSuburb(string suburbName)
        {
            var suburbResponseDto = new List<SuburbSearchResponseDto>();
            var data = await _httpClient.FindSuburb(suburbName).Result.Content.ReadFromJsonAsync<IEnumerable<SuburbSearch>>();
            foreach (var suburb in data)
            {
                string input = suburb.Name;
                string pattern = @"\sExt\s\d+$";
                suburb.Name = Regex.Replace(input, pattern, "");

                var result = _mapper.Map<SuburbSearchResponseDto>(suburb);
                var province = await _httpClient.GetProvinceByName(suburb.ProvinceName).Result.Content.ReadFromJsonAsync<Province>();
                result.Province = province;
                var municipality = await _httpClient.GetMunicipalityByName(province.ProvinceId, suburb.MunicipalityName).Result.Content.ReadFromJsonAsync<Municipality>();
                result.Municipality = municipality;
                suburbResponseDto.Add(result);
            }
            return suburbResponseDto.DistinctBy(x => x.Name);
        }

        public Task<IEnumerable<ScheduleDto>> GetSchedule(int municipalityId, int blockId, int days, int stage)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<SuburbData>> GetSuburbs(int? blockId = null)
        {
            throw new System.NotImplementedException();
        }
    }
}