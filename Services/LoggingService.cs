using EskomCalendarApi.Models.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EskomCalendarApi.Services
{
    public class LoggingService
    {


        public LoggingService()
        {
            if (!Directory.Exists("./Data"))
            {
                Directory.CreateDirectory("./Data");
            }

        }
        public void SuburbAdded(SuburbItem message)
        {
            DateTime myDate = DateTime.Now;

            if (!File.Exists("./Data/suburb.json"))
            {
                var f = File.Create("./Data/suburb.json");
                f.Close();
                TextWriter tw = new StreamWriter("./Data/suburb.json", true);
                tw.WriteLine("[]");
                tw.Close();
            }

            List<SuburbItem> items = new List<SuburbItem>();
            using (StreamReader r = new StreamReader("./Data/suburb.json"))
            {
                string json = r.ReadToEnd();
                items = JsonSerializer.Deserialize<List<SuburbItem>>(json);
            }
            items.Add(message);
            using (StreamWriter sw = new StreamWriter("./Data/suburb.json"))
            {
                var data = JsonSerializer.Serialize(items);
                sw.Write(data);
            }
        }

        public void SuburbViewed(SuburbItem message)
        {
            DateTime myDate = DateTime.Now;

            if (!File.Exists("./Data/suburbViewed.json"))
            {
                var f = File.Create("./Data/suburbViewed.json");
                f.Close();
                TextWriter tw = new StreamWriter("./Data/suburbViewed.json", true);
                tw.WriteLine("[]");
                tw.Close();
            }

            List<SuburbItem> items = new List<SuburbItem>();
            using (StreamReader r = new StreamReader("./Data/suburbViewed.json"))
            {
                string json = r.ReadToEnd();
                items = JsonSerializer.Deserialize<List<SuburbItem>>(json);
            }
            items.Add(message);
            using (StreamWriter sw = new StreamWriter("./Data/suburbViewed.json"))
            {
                var data = JsonSerializer.Serialize(items);
                sw.Write(data);
            }
        }

        public void SuburbRemoved(SuburbItem message)
        {
            DateTime myDate = DateTime.Now;

            if (!File.Exists("./Data/suburbRemoved.json"))
            {
                var f = File.Create("./Data/suburbRemoved.json");
                f.Close();
                TextWriter tw = new StreamWriter("./Data/suburbRemoved.json", true);
                tw.WriteLine("[]");
                tw.Close();
            }

            List<SuburbItem> items = new List<SuburbItem>();
            using (StreamReader r = new StreamReader("./Data/suburbRemoved.json"))
            {
                string json = r.ReadToEnd();
                items = JsonSerializer.Deserialize<List<SuburbItem>>(json);
            }
            items.Add(message);
            using (StreamWriter sw = new StreamWriter("./Data/suburbRemoved.json"))
            {
                var data = JsonSerializer.Serialize(items);
                sw.Write(data);
            }
        }

        public void Installed(InstalledItem message)
        {
            DateTime myDate = DateTime.Now;
            message.ActionDate = myDate;

            if (!File.Exists("./Data/installed.json"))
            {
                var f = File.Create("./Data/installed.json");
                f.Close();
                TextWriter tw = new StreamWriter("./Data/installed.json", true);
                tw.WriteLine("[]");
                tw.Close();
            }

            List<InstalledItem> items = new List<InstalledItem>();
            using (StreamReader r = new StreamReader("./Data/installed.json"))
            {
                string json = r.ReadToEnd();
                items = JsonSerializer.Deserialize<List<InstalledItem>>(json);
            }
            items.Add(message);
            using (StreamWriter sw = new StreamWriter("./Data/installed.json"))
            {
                var data = JsonSerializer.Serialize(items);
                sw.Write(data);
            }
        }

        public void UnInstalled(InstalledItem message)
        {
            DateTime myDate = DateTime.Now;
            message.ActionDate = myDate;
            if (!Directory.Exists("./Data"))
            {
                var d = Directory.CreateDirectory("./Data");
            }

            if (!File.Exists("./Data/uninstalled.json"))
            {
                var f = File.Create("./Data/uninstalled.json");
                f.Close();
                TextWriter tw = new StreamWriter("./Data/uninstalled.json", true);
                tw.WriteLine("[]");
                tw.Close();
            }

            List<InstalledItem> items = new List<InstalledItem>();
            using (StreamReader r = new StreamReader("./Data/uninstalled.json"))
            {
                string json = r.ReadToEnd();
                items = JsonSerializer.Deserialize<List<InstalledItem>>(json);
            }
            items.Add(message);
            using (StreamWriter sw = new StreamWriter("./Data/uninstalled.json"))
            {
                var data = JsonSerializer.Serialize(items);
                sw.Write(data);
            }
        }
    }
}
