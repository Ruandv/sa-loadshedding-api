using EskomCalendarApi.Models.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace EskomCalendarApi.Services
{
    public class LoggingService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions();

        public LoggingService()
        {
            options.WriteIndented = true;

            if (!Directory.Exists("./Data"))
            {
                Directory.CreateDirectory("./Data");
            }

        }
        public void SuburbAdded(SuburbItem message)
        {
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

            var data = JsonSerializer.Serialize(items, options);
            SaveFile("./Data/suburb.json", data);
        }

        public void SuburbRemoved(SuburbItem message)
        {
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

            var idx = items.FindIndex(x => x.UserToken == message.UserToken && x.SuburbName == message.SuburbName);
            if (idx > -1)
            {
                items.RemoveAt(idx);
            }
            var data = JsonSerializer.Serialize(items, options);
            SaveFile("./Data/suburb.json", data);
        }

        public void SuburbViewed(SuburbItem message)
        {
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

            //find the item for today
            var idx = items.Find(x => x.UserToken == message.UserToken && x.SuburbName == message.SuburbName && x.ActionDate.Date.DayOfYear == message.ActionDate.Date.DayOfYear);
            if (idx != null)
            {
                idx.Viewed = idx.Viewed + 1;
            }
            else
            {
                message.Viewed = 1;
                items.Add(message);
            }

            var data = JsonSerializer.Serialize(items, options);
            SaveFile("./Data/suburbViewed.json", data);
        }


        public void Installed(InstalledItem message)
        {
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
            var idx = items.FindAll(x => x.UserToken == message.UserToken);
            if (idx.Count > 0)
            {
                idx.ForEach(x => x.Message = message.Message);
            }
            else
            {
                items.Add(message);
            }
            var data = JsonSerializer.Serialize(items, options);
            SaveFile("./Data/installed.json", data);
        }

        private void RemoveFromInstalled(string userToken)
        {
            if (!File.Exists("./Data/installed.json"))
            {
                var f = File.Create("./Data/installed.json");
                f.Close();
                TextWriter tw = new StreamWriter("./Data/installed.json", true);
                tw.WriteLine("[]");
                tw.Close();
                return;
            }

            List<InstalledItem> items = new List<InstalledItem>();
            using (StreamReader r = new StreamReader("./Data/installed.json"))
            {
                string json = r.ReadToEnd();
                items = JsonSerializer.Deserialize<List<InstalledItem>>(json);
            }
            var itemToRemove = items.FirstOrDefault(x => x.UserToken == userToken);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
            }
            var data = JsonSerializer.Serialize(items, options);
            SaveFile("./Data/installed.json", data);
        }

        private void SaveFile(string path, string content)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(content);
            }
        }

        public void UnInstalled(InstalledItem message)
        {
            if (!Directory.Exists("./Data"))
            {
                var d = Directory.CreateDirectory("./Data");
            }
            // try to remove the userfrom the installed
            RemoveFromInstalled(message.UserToken);
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

            var data = JsonSerializer.Serialize(items, options);
            SaveFile("./Data/uninstalled.json", data);
        }
    }
}
