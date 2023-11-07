using System;
using System.Text.Json.Serialization;

namespace Models.Eskom
{
    public class Schedule
    {
        public string Time { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Stage { get; set; }
        public int Day1 { get; set; }
        public int Day2 { get; set; }
        public int Day3 { get; set; }
        public int Day4 { get; set; }
        public int Day5 { get; set; }
        public int Day6 { get; set; }
        public int Day7 { get; set; }
        public int Day8 { get; set; }
        public int Day9 { get; set; }
        public int Day10 { get; set; }
        public int Day11 { get; set; }
        public int Day12 { get; set; }
        public int Day13 { get; set; }
        public int Day14 { get; set; }
        public int Day15 { get; set; }
        public int Day16 { get; set; }
        public int Day17 { get; set; }
        public int Day18 { get; set; }
        public int Day19 { get; set; }
        public int Day20 { get; set; }
        public int Day21 { get; set; }
        public int Day22 { get; set; }
        public int Day23 { get; set; }
        public int Day24 { get; set; }
        public int Day25 { get; set; }
        public int Day26 { get; set; }
        public int Day27 { get; set; }
        public int Day28 { get; set; }
        public int Day29 { get; set; }
        public int Day30 { get; set; }
        public int Day31 { get; set; }
    }

    public class ScheduleDto
    {
        [JsonPropertyName("blockId")]
        public int BlockId { get; set; }
        [JsonPropertyName("stage")]
        public int Stage { get; set; }
        [JsonPropertyName("dayOfMonth")]
        public DateTime DayOfMonth { get; set; }
        [JsonPropertyName("start")]
        public TimeSpan Start { get; set; }
        [JsonPropertyName("end")]
        public TimeSpan End { get; set; }

        public override string ToString()
        {
            return Stage.ToString() + " " + DayOfMonth.ToString("dd") + " " + Start.ToString() + " " + End.ToString() + " ";
        }
    }
}
