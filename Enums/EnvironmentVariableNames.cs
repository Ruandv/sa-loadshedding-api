using System.ComponentModel;

namespace EskomCalendarApi.Enums
{
    public enum EnvironmentVariableNames
    {
        ALLOWINGHOSTS,
        ALLOWEDKEYS,
        ESKOM_CALENDAR_BASE_URL,
        ESP_BASE_URL,
        ESKOM_SITE_BASE_URL,
        FIREBASE_BASE_URL,
        GOOGLE_APPLICATION_CREDENTIALS
    }

    public enum ExtensionMessageType
    {
        [Description("INSTALLED")]
        INSTALLED,
        [Description("UPDATED")]
        UPDATED,
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
