namespace Services
{
  public interface IReadWriteService
  {
    abstract void WriteFile(string path, string content);
    abstract string ReadFile(string path);
  }
}