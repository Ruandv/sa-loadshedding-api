using System;

namespace EskomCalendarApi.Models.Logging
{
    public class MessageDetails
    {
        public string UserToken { get; set; }
    }

    public class SuburbItem : MessageDetails
    {
        public string SuburbName { get; set; }
    }

    public class InstalledItem : MessageDetails
    {
        public DateTime ActionDate { get; set; }
        public string Message { get; set; }
    }
}
