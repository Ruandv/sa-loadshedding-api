using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace EskomCalendarApi.Enums
{
    public enum EnvironmentVariableNames
    {
        ESKOM_CALENDAR_BASE_URL,
        ESKOM_SITE_BASE_URL,
        FIREBASE_BASE_URL,
        GOOGLE_APPLICATION_CREDENTIALS
    }

    public enum ExtensionMessageType
    {
        [Description("INSTALLED")]
        INSTALLED,
        [Description("UNINSTALLED")]
        UNINSTALLED,
        [Description("SUBURBADDED")]
        SUBURBADDED,
        [Description("SUBURBREMOVED")]
        SUBURBREMOVED,
        [Description("SUBURBVIEWED")]
        SUBURBVIEWED,
        [Description("DAYSCHANGED")]
        DAYSCHANGED,
        [Description("SEARCHED")]
        SEARCHED,
    }
}
