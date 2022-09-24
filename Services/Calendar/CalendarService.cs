using System.Threading.Tasks;
using System;
using System.IO;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using EskomCalendarApi.Models.Calendar;

namespace EskomCalendarApi.Services.Calendar
{
    public interface ICalendarService
    {
        Task<string> GetCalendarData(string calendarName);
        Task<MachineDataDto> GetMachineData(int lastRecord, int recordsToRetrieve);
        Task<MachineDataDto> GetDataByArea(string areaDescription, int lastRecord = 0, int recordsToRetrieve = 100);
        Task<MachineDataDto> GetDataByAreaDateTime(string areaDescription, DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<SuburbData>> GetCalendarSuburbs(string calendarName);
    }
    public class CalendarService : ICalendarService
    {
        private readonly CalendarHttpClient _httpClient;
        private readonly IEskomService _eskomService;
        private List<MyMachineData> machineFileData = new List<MyMachineData>();

        public CalendarService(CalendarHttpClient myHttpClient, IEskomService eskomService)
        {
            _httpClient = myHttpClient;
            _eskomService = eskomService;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                IgnoreReferences = true,
            };
            List<MachineData> myMachineData = new List<MachineData>();

            using (var reader = new StreamReader(_httpClient.GetMachineFriendlyFile().Result))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    myMachineData = csv.GetRecords<MachineData>().ToList();
                }
            }

            foreach (MachineData machineData in myMachineData)
            {
                machineFileData.Add(GetProvince(machineData));
            }
        }

        private MyMachineData GetProvince(MachineData m)
        {
            var a = new List<string>(){
                "city-of-cape-town","city-power","eastern-cape", "free-state","kwazulu-natal",
                "gauteng","limpopo","mpumalanga","north-west","northern-cape","western-cape"};
            var myData = new MyMachineData();
            myData.start = m.start;
            myData.finsh = m.finsh;
            myData.source = m.source;
            myData.stage = m.stage;

            myData.area_name = m.area_name;
            myData.province = a.Find(x => x.Length < m.area_name.Length && x == m.area_name.Substring(0, x.Length));
            myData.block = m.area_name.Substring(myData.province.Length + 1).Replace(".ics", "");
            return myData;
        }

        public async Task<MachineDataDto> GetMachineData(int lastRecord, int recordsToRetrieve)
        {

            var d = new MachineDataDto();

            d.data = machineFileData.Skip(lastRecord).Take(recordsToRetrieve).ToList();
            d.lastRecord = (lastRecord + recordsToRetrieve) > machineFileData.Count() ? machineFileData.Count() : lastRecord + recordsToRetrieve;
            d.totalRecords = machineFileData.Count();
            return await Task.FromResult(d);
        }
        public async Task<MachineDataDto> GetDataByArea(string areaDescription, int lastRecord = 0, int recordsToRetrieve = 100)
        {
            var data = machineFileData.ToList().Where(x => x.area_name.Contains(areaDescription)).ToList();
            var dto = new MachineDataDto();
            dto.data = data;
            dto.totalRecords = data.Count();
            dto.lastRecord = (lastRecord + recordsToRetrieve) > data.Count() ? data.Count() : lastRecord + recordsToRetrieve;
            return await Task.FromResult(dto);
        }
        public async Task<string> GetCalendarData(string calendarName)
        {
            try
            {
                var data = await _httpClient.GetCalendarByName(calendarName);
                return await data.Content.ReadAsStringAsync();
            }
            catch
            {
                return string.Empty;
            }
        }
        public async Task<MachineDataDto> GetDataByAreaDateTime(string areaDescription, DateTime startDateTime, DateTime endDateTime)
        {
            var data = machineFileData.ToList().Where(x => x.area_name == areaDescription && (DateTime.Parse(x.start).Date >= startDateTime.Date && DateTime.Parse(x.finsh).Date <= endDateTime.Date)).ToList();
            var dto = new MachineDataDto();
            dto.data = data;
            dto.totalRecords = data.Count();
            dto.lastRecord = data.Count();
            return await Task.FromResult(dto);
        }
        public async Task<IEnumerable<SuburbData>> GetCalendarSuburbs(string calendarName)
        {
            // try to sanitize the data by province municipality blockId
            //Currently we only support Tswane and COJ
            if (calendarName.Contains("Gauteng-tshwane", StringComparison.InvariantCultureIgnoreCase))
            {
                var blockId = calendarName.Substring(calendarName.LastIndexOf("-") + 1, calendarName.Length - 4); //exclude the extension
                return await _eskomService.GetSuburbsByMunicipality(167, int.Parse(blockId));
            }
            else if (calendarName.Contains("city-power", StringComparison.InvariantCultureIgnoreCase))
            {
                var blockId = calendarName.Substring(calendarName.LastIndexOf("-") + 1, calendarName.LastIndexOf(".") - calendarName.LastIndexOf("-") - 1); //exclude the extension
                return await _eskomService.GetSuburbsByMunicipality(166, int.Parse(blockId));
            }
            else
            {
                throw new CalendarSuburbsNotImplementedException("The only supported calendars are Gauteng-tshwane-* and city-power-*");
            }
        }
    }
}