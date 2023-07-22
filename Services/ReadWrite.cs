using System.IO;

namespace Services
{
  public abstract class ReadWrite
  {
    private void VerifyPath(string path)
    {
      var location = path.Split('/');
      var p = "";
      int i = 0;
      while (i < location.Length - 1)
      {
        p += location[i];
        if (!Directory.Exists(p))
        {
          var d = Directory.CreateDirectory("./Cache");
        }
        p += "/";
        i++;
      }

      if (!File.Exists(p + location[location.Length - 1]))
      {
        var f = File.Create(p + location[location.Length - 1]);
        f.Close();
        TextWriter tw = new StreamWriter(p + location[location.Length - 1], true);
        tw.WriteLine("[]");
        tw.Close();
      }
    }

    public void WriteFile(string path, string content)
    {
      VerifyPath(path);
      using (StreamWriter sw = new StreamWriter(path))
      {
        sw.Write(content);
      }
    }

    public string ReadFile(string path)
    {
      VerifyPath(path);
      using (StreamReader sr = new StreamReader(path))
      {
        return sr.ReadToEnd();
      }
    }

  }
}
