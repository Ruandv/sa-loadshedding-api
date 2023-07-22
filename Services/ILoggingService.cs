using Models.Logging;

namespace Services
{
  public interface ILoggingService
  {
    void Installed(InstalledItem message);
    void SuburbAdded(SuburbItem message);
    void SuburbRemoved(SuburbItem message);
    void SuburbViewed(SuburbItem message);
    void UnInstalled(InstalledItem message);
  }
}