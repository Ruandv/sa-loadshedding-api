using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Services
{
  public class CacheService : ReadWrite, ICacheService
  {
    private readonly ILogger<EskomService> _logger;
    public CacheService(ILogger<EskomService> logger)
    {
      _logger = logger;
    }

    public void SetCache(string fileName, string content)
    {
      if (!Directory.Exists("Cache"))
      {
        var d = Directory.CreateDirectory("./Cache");
      }
      if (!File.Exists("./Cache/" + fileName + ".json"))
      {
        var f = File.Create("./Cache/" + fileName + ".json");
        f.Close();
        TextWriter tw = new StreamWriter("./Cache/" + fileName + ".json", true);
        tw.WriteLine("[]");
        tw.Close();
      }
      _logger.LogInformation("Updated Cache for  : " + fileName + ".json");
      WriteFile("./Cache/" + fileName + ".json", content);
    }

    public string GetCache(string fileName, TimeSpan duration)
    {
      var val = File.GetLastWriteTime("./Cache/" + fileName + ".json");
      var today = DateTime.Now;
      var diff = today - val;
      if (diff.TotalMinutes > duration.TotalMinutes)
      {
        _logger.LogWarning("Cache is OLD  : " + fileName + ".json");
        return null;
      }
      return ReadFile("./Cache/" + fileName + ".json");
    }
  }
}
