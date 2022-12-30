using EskomCalendarApi.Models.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace EskomCalendarApi.Services
{
    public class LoggingService
    {


        public LoggingService()
        {

        }

        public void Installed(string message)
        {
            DateTime myDate = DateTime.Now;
            if (!Directory.Exists("./Data"))
            {
                var d = Directory.CreateDirectory("./Data");
            }

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
                items = JsonConvert.DeserializeObject<List<InstalledItem>>(json);
            }
            items.Add(new InstalledItem() { InstalledDate = myDate, Detail = message });
            using (StreamWriter sw = new StreamWriter("./Data/installed.json"))
            {
                var data = JsonConvert.SerializeObject(items);
                sw.Write(data);
            }
        }
    
        public void UnInstalled(string message)
        {
            DateTime myDate = DateTime.Now;
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
                items = JsonConvert.DeserializeObject<List<InstalledItem>>(json);
            }
            items.Add(new InstalledItem() { InstalledDate = myDate, Detail = message });
            using (StreamWriter sw = new StreamWriter("./Data/uninstalled.json"))
            {
                var data = JsonConvert.SerializeObject(items);
                sw.Write(data);
            }
        }
    }
}
