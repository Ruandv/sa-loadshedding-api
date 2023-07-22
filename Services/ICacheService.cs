namespace Services
{
  public interface ICacheService
  {
    string GetCache(string fileName);
    void SetCache(string fileName, string content);
  }
}