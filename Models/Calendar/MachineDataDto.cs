﻿using System.Collections.Generic;

namespace EskomCalendarApi.Models.Calendar
{
    public class MachineDataDto
    {
        public IEnumerable<MyMachineData> data { get; set; }
        public int lastRecord { get; set; }
        public int totalRecords { get; set; }
    }
}
