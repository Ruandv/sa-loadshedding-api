using System;

namespace Services
{
  public interface ICacheService
  {
    string GetCache(string fileName, TimeSpan duration);
    void SetCache(string fileName, string content);
  }
}