using System.Collections.Generic;

namespace EskomCalendarApi.Models.Calendar
{
    public class MachineDataDto
    {
        public IEnumerable<MyMachineData> data { get; set; }
        public int lastRecord { get; set; }
        public int totalRecords { get; set; }
    }

    public class MachineDataGroupedDto
    {
        public IEnumerable<MyMachineDataGrouped> data { get; set; }
    }

    public class MyMachineDataGrouped
    {
        public string area_name { get; set; }
        public string province { get; set; }
        public string block { get; set; }
    }
}
