using System.Threading.Tasks;
using System.Collections.Generic;
using HttpClients;
using System.Net.Http.Json;
using Models.Eskom;
using Models.Eskom.ResponseDto;
using System.Linq;
using AutoMapper;
using System.Text.RegularExpressions;
using System;
using Microsoft.AspNetCore.Http;
using System.Net;
using Utilities;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace Services
{
  public interface IMunicipalService
  {
    //Task<IEnumerable<SuburbData>> GetSuburbs(int? blockId = null);
    Task<IEnumerable<SuburbSearchResponseDto>> GetSuburbListByMunicipality(int provinceId, int municipalityId);
    Task<IEnumerable<SuburbSearchResponseDto>> FindSuburb(string suburbName, int? provinceId);
    Task<IEnumerable<ScheduleDto>> GetSchedule(int municipalityId, int blockId, int days, int stage);
  }

  public interface IProvincialService
  {
    Task<IEnumerable<Municipality>> GetMunicipalities(int provinceId);
  }
  public interface INationalService
  {
    Task<IEnumerable<Province>> GetProvinces();
    Task<int> GetStatus();
  }

  public interface IEskomService : INationalService, IProvincialService, IMunicipalService
  {


  }

  public class EskomService : IEskomService
  {
    private readonly EskomHttpClient2 _httpClient;
    private readonly IMapper _mapper;
    private readonly ILogger<EskomService> _logger;
    private readonly ICacheService _cacheService;
    private ILoggingService _logService;

    public EskomService(EskomHttpClient2 myHttpClient, IMapper mapper, ILogger<EskomService> logger, ICacheService cacheService, ILoggingService logService)
    {
      _httpClient = myHttpClient;
      _mapper = mapper;
      _logger = logger;
      _cacheService = cacheService;
      _logService = logService;
    }

    public async Task<IEnumerable<Province>> GetProvinces()
    {
      return await _httpClient.GetProvinceList().Result.Content.ReadFromJsonAsync<IEnumerable<Province>>();
    }
    public async Task<IEnumerable<Municipality>> GetMunicipalities(int provinceId)
    {
      try
      {
        var res = await _httpClient.GetMunicipalityList(provinceId).Result.Content.ReadAsStringAsync();
        _logger.LogInformation("GetMunicipalityList Result : " + res);
        var dta = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Municipality>>(res);
        _cacheService.SetCache("GetMunicipalities_" + provinceId, res);
        return dta;
      }
      catch (Exception ex)
      {
        // log the issue
        _logger.LogError(ex.Message);
        // look in the files if we have a list of municipalities for this provinceId
        var res = _cacheService.GetCache("GetMunicipalities_" + provinceId);
        var dta = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Municipality>>(res);

        return dta;
      }
    }
    public async Task<IEnumerable<SuburbSearchResponseDto>> GetSuburbListByMunicipality(int provinceId, int municipalityId)
    {
      try
      {
        var suburbResponseDto = new List<SuburbSearchResponseDto>();
        var municipalities = await _httpClient.GetMunicipalityList(provinceId).Result.Content.ReadFromJsonAsync<IEnumerable<Municipality>>();
        var municipality = municipalities.ToList().First(x => x.MunicipalityId == municipalityId);
        var list = await _httpClient.GetSuburbListByMunicipality(municipality.MunicipalityId).Result.Content.ReadFromJsonAsync<SuburbDataResult>();
        foreach (var suburb in list.Results)
        {
          string input = suburb.Name;
          string pattern = @"\sExt\s\d+$";
          suburb.Name = Regex.Replace(input, pattern, "");

          var result = _mapper.Map<SuburbSearchResponseDto>(suburb);
          result.IsEskomClient = suburb.Total > 0;
          result.BlockId = suburb.Id;
          result.Name += result.IsEskomClient ? "*" : "";
          var province = await _httpClient.GetProvinceList().Result.Content.ReadFromJsonAsync<Province[]>();

          result.Province = province.First(x => x.ProvinceId == provinceId);
          result.Municipality = municipality;

          if (new List<int> { 166, 167, 168 }.Contains(municipalityId) && result.IsEskomClient == false)
          {
            var dt = Transformers.GetBlockIdFromJSON("./services/Data/JSONData/Municipality_" + municipalityId + ".json", suburb.Name);
            if (dt == null)
              continue;
            result.BlockId = int.Parse(dt.ToList()[0].BlockId);
            result.IsEskomClient = true;
          }

          if (result.IsEskomClient)
          {
            suburbResponseDto.Add(result);
          }
        }
        if (new List<int> { 166, 167, 168 }.Contains(municipalityId))
        {
          // check if there are any suburbs that is in the file thats not listed in the current array. 
          suburbResponseDto.AddRange(Transformers.MergeEskomData("./services/Data/JSONData/Municipality_" + municipalityId + ".json", suburbResponseDto));
        }
        var res = suburbResponseDto.DistinctBy(x => x.Name);
        _cacheService.SetCache("GetSuburbListByMunicipality_" + provinceId + "_" + municipalityId, System.Text.Json.JsonSerializer.Serialize(res));
        return res;
      }
      catch (Exception ex)
      {
        // log the issue
        _logger.LogError(ex.Message);
        var res = _cacheService.GetCache("GetSuburbListByMunicipality_" + provinceId + "_" + municipalityId);
        var dta = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<SuburbSearchResponseDto>>(res);
        return dta;
      }
    }

    public async Task<IEnumerable<SuburbSearchResponseDto>> FindSuburb(string suburbName, int? municipalityId)
    {
      var suburbResponseDto = new List<SuburbSearchResponseDto>();
      var data = await _httpClient.FindSuburb(suburbName).Result.Content.ReadFromJsonAsync<IEnumerable<SuburbSearch>>();

      foreach (var suburb in data)
      {
        string input = suburb.Name;
        string pattern = @"\sExt\s\d+$";
        suburb.Name = Regex.Replace(input, pattern, "");

        var result = _mapper.Map<SuburbSearchResponseDto>(suburb);
        result.IsEskomClient = suburb.Total > 0;

        var province = await _httpClient.GetProvinceByName(suburb.ProvinceName).Result.Content.ReadFromJsonAsync<Province>();

        result.Province = province;
        var municipality = await _httpClient.GetMunicipalityByName(province.ProvinceId, suburb.MunicipalityName).Result.Content.ReadFromJsonAsync<Municipality>();
        result.Municipality = municipality;
        if (municipalityId.HasValue && municipality.MunicipalityId != municipalityId.Value)
        {
          //continue to the next item because this one is not relevant for the selected province
          continue;
        }

        if (municipalityId.HasValue && new List<int> { 166, 167, 168 }.Contains(municipalityId.Value))
        {
          var dt = Transformers.GetBlockIdFromJSON("./services/Data/JSONData/Municipality_" + municipalityId + ".json", suburb.Name);
          if (dt == null)
            continue;
          result.BlockId = int.Parse(dt.ToList()[0].BlockId);
          result.IsEskomClient = true;
        }

        if (result.IsEskomClient)
        {
          suburbResponseDto.Add(result);
        }
      }
      return suburbResponseDto.DistinctBy(x => x.Name);
    }

    public async Task<IEnumerable<ScheduleDto>> GetSchedule(int municipalityId, int blockId, int days, int stage)
    {
      var myMunicipalityList = new int[] { 166, 167, 168 };
      if (myMunicipalityList.Contains(municipalityId))
      {
        var dt = Transformers.GetDataTableFromCsv("./services/data/" + municipalityId + ".csv", stage, blockId, days);
        if (dt.Any())
        {
          return dt;
        }
      }

      var result = await _httpClient.GetSchedule(blockId, stage);
      if (result.StatusCode == HttpStatusCode.OK)
      {
        return Transformers.HtmlDataToJson(await result.Content.ReadAsStringAsync(), stage, blockId, days);
      }
      else
      {
        return new List<ScheduleDto>();
      }
    }

    public async Task<int> GetStatus()
    {
      try
      {
        var res = await _httpClient.GetStatus().Result.Content.ReadAsStringAsync();
        _logger.LogInformation(res);
        int.TryParse(res, out int intValue);
        _cacheService.SetCache("GetStatus", System.Text.Json.JsonSerializer.Serialize<int>(intValue));
        return intValue;
      }
      catch (AggregateException ex)
      {
        // log the issue
        _logger.LogError(ex.Message);
        var res = _cacheService.GetCache("GetStatus");
        var dta = System.Text.Json.JsonSerializer.Deserialize<int>(res);
        return dta;
      }
    }
  }
}