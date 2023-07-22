using System.IO;

namespace Services
{
  public class CacheService : ReadWrite, ICacheService
  {

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
      WriteFile("./Cache/" + fileName + ".json", content);
    }

    public string GetCache(string fileName)
    {
      return ReadFile("./Cache/" + fileName + ".json");
    }
  }
}
