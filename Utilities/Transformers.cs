using CSVFile;
using EskomCalendarApi.Models.Eskom;
using HtmlAgilityPack;
using Models.Eskom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Utilities
{
    public static class Transformers
    {
        public static IEnumerable<ScheduleDto> HtmlDataToJson(string htmlData, int stage, int blockId, int numberOfDays)
        {
            List<ScheduleDto> resultList = new List<ScheduleDto>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlData);
            var days = doc.DocumentNode.Descendants("div").Where(d => d.HasClass("scheduleDay"));
            days.Take(numberOfDays).ToList().ForEach(d =>
            {
                var dateNode = d.Descendants("div").FirstOrDefault(d => d.HasClass("dayMonth"));
                var dateString = dateNode.InnerText.Trim();
                DateTime result = DateTime.ParseExact(dateString, "ddd, dd MMM", CultureInfo.InvariantCulture);
                var slotNodes = d.Descendants("a").Where(d => d.HasAttributes && d.Attributes["onclick"] != null);
                slotNodes.ToList().ForEach(s =>
                {
                    var slotData = s.InnerText.Split(" - ");

                    var start = new DateTime().AddHours(double.Parse(slotData[0].Split(":")[0])).AddMinutes(double.Parse(slotData[0].Split(":")[1]));
                    var end = new DateTime().AddHours(double.Parse(slotData[1].Split(":")[0])).AddMinutes(double.Parse(slotData[1].Split(":")[1]));
                    if (short.Parse(slotData[1].Split(":")[0]) < start.Hour)
                    {
                        var v = double.Parse(slotData[1].Split(":")[0]);
                        var v2 = double.Parse(slotData[1].Split(":")[1]);
                        end = new DateTime().AddHours(v).AddMinutes(v2);
                    }
                    resultList.Add(new ScheduleDto { BlockId = blockId, Stage = stage, DayOfMonth = result, Start = start.TimeOfDay, End = end.TimeOfDay });
                });
            });
            return resultList;
        }

        public static IEnumerable<ScheduleDto> GetDataTableFromCsv(string path, int? stage, int? blockId, int? days)
        {
            var settings = new CSVSettings()
            {
                FieldDelimiter = ',',
                TextQualifier = '\'',
                ForceQualifiers = true
            };
            try
            {
                var sr = new StreamReader(path);

                using (CSVReader cr = new CSVReader(sr, settings))
                {
                    var d = cr.Deserialize<Schedule>();
                    var dto = new List<ScheduleDto>();

                    d.ToList().ForEach(x =>
                    {
                        dto.Add(new ScheduleDto() { BlockId = x.Day1, DayOfMonth = DateTime.Today.Day > 1 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day2, DayOfMonth = DateTime.Today.Day > 2 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 2).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 2), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day3, DayOfMonth = DateTime.Today.Day > 3 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 3).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 3), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day4, DayOfMonth = DateTime.Today.Day > 4 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 4).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 4), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day5, DayOfMonth = DateTime.Today.Day > 5 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 5).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 5), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day6, DayOfMonth = DateTime.Today.Day > 6 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 6).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 6), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day7, DayOfMonth = DateTime.Today.Day > 7 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 7).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 7), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day8, DayOfMonth = DateTime.Today.Day > 8 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 8).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 8), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day9, DayOfMonth = DateTime.Today.Day > 9 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 9).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 9), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day10, DayOfMonth = DateTime.Today.Day > 10 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 10).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 10), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day11, DayOfMonth = DateTime.Today.Day > 11 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 11).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 11), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day12, DayOfMonth = DateTime.Today.Day > 12 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 12).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 12), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day13, DayOfMonth = DateTime.Today.Day > 13 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 13).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 13), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day14, DayOfMonth = DateTime.Today.Day > 14 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 14).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 14), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day15, DayOfMonth = DateTime.Today.Day > 15 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 15).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 15), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day16, DayOfMonth = DateTime.Today.Day > 16 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 16).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 16), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day17, DayOfMonth = DateTime.Today.Day > 17 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 17).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 17), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day18, DayOfMonth = DateTime.Today.Day > 18 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 18).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 18), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day19, DayOfMonth = DateTime.Today.Day > 19 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 19).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 19), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day20, DayOfMonth = DateTime.Today.Day > 20 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 20).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 20), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day21, DayOfMonth = DateTime.Today.Day > 21 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 21).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 21), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day22, DayOfMonth = DateTime.Today.Day > 22 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 22).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 22), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day23, DayOfMonth = DateTime.Today.Day > 23 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 23).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 23), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day24, DayOfMonth = DateTime.Today.Day > 24 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 24).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 24), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day25, DayOfMonth = DateTime.Today.Day > 25 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 25).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 25), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day26, DayOfMonth = DateTime.Today.Day > 26 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 26).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 26), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day27, DayOfMonth = DateTime.Today.Day > 27 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 27).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 27), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        dto.Add(new ScheduleDto() { BlockId = x.Day28, DayOfMonth = DateTime.Today.Day > 28 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 28).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 28), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                        if (DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) > 28)
                        {
                            dto.Add(new ScheduleDto() { BlockId = x.Day29, DayOfMonth = DateTime.Today.Day > 29 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 29).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 29), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                            if (DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) > 29)
                            {
                                dto.Add(new ScheduleDto() { BlockId = x.Day30, DayOfMonth = DateTime.Today.Day > 30 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 30).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 30), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                            }
                            if (DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) > 30)
                            {
                                dto.Add(new ScheduleDto() { BlockId = x.Day31, DayOfMonth = DateTime.Today.Day > 31 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 31).AddMonths(1) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 31), Stage = x.Stage, Start = x.Start.TimeOfDay, End = x.End.TimeOfDay });
                            }
                        }
                    });

                    var dayList = new List<DateTime>();
                    if (days.HasValue)
                    {
                        for (int i = 0;i <= days.Value;i++)
                        {
                            dayList.Add(DateTime.Today.AddDays(i));
                        }
                    }
                    else
                    {
                        dayList.Add(DateTime.Today.AddDays(1));
                    }

                    var rees = dto.ToList();

                    if (blockId.HasValue)
                    {
                        rees = rees.Where(x => x.BlockId == blockId.Value).ToList();
                    }

                    if (stage.HasValue)
                    {
                        rees = rees.Where(x => x.Stage <= stage.Value).ToList();
                    }

                    rees = rees.Where(x => dayList.IndexOf(x.DayOfMonth) >= 0).Distinct()
                        .OrderBy(x => x.DayOfMonth.Year)
                        .ThenBy(x => x.DayOfMonth.Month)
                        .ThenBy(x => x.DayOfMonth.Day)
                        .ThenBy(x => x.Start).ToList()
                        ;
                    return rees;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return null;
        }

        public static IEnumerable<SuburbData> GetBlockIdFromJSON(string path, string suburbName)
        {
            String jsonString = new StreamReader(path).ReadToEnd();

            // use below syntax to access JSON file
            var res = JsonSerializer.Deserialize<List<SuburbData>>(jsonString).Where(x => x.SubName.Contains(suburbName));
            if(res.Count()<1)
                return null;
            return res;

        }
    }
}
