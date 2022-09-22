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
    }
    public class CalendarService : ICalendarService
    {
        private readonly CalendarHttpClient _httpClient;
        private IEnumerable<MachineData> machineFileData;

        public CalendarService(CalendarHttpClient myHttpClient)
        {
            _httpClient = myHttpClient;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
            };
            using (var reader = new StreamReader(_httpClient.GetMachineFriendlyFile().Result))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    machineFileData = csv.GetRecords<MachineData>().ToList();
                }
            }
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
    }
}