using System;

namespace Models.Esp
{
    public class AreasObject
    {
        public Area[] Areas { get; set; }
    }
    public class Area
    {
        public string id { get; set; }
        public string name { get; set; }
        public string region { get; set; }
    }
    public class StatusObject
    {
        public Status Status { get; set; }
    }

    public class Status
    {
        public StatusItem[] StatusItems { get; set; }
    }
    public class StatusItem
    {
        public string name { get; set; }
        public string next_stages { get; set; }
        public string stage { get; set; }
        public DateTime stage_updated { get; set; }
    }
}