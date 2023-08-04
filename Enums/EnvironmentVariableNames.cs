using System.ComponentModel;

namespace Enums
{
  public enum EnvironmentVariableNames
  {
    TOKEN, 
    ALLOWINGHOSTS,
    ALLOWEDKEYS,
    ESP_BASE_URL,
    ESKOM_SITE_BASE_URL,
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
