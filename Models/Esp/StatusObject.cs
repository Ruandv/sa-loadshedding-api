using System;
using System.Collections.Generic;

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
        public Status status { get; set; }
    }
    public class Capetown
    {
        public string name { get; set; }
        public List<NextStage> next_stages { get; set; }
        public string stage { get; set; }
        public DateTime stage_updated { get; set; }
    }

    public class Eskom
    {
        public string name { get; set; }
        public List<NextStage> next_stages { get; set; }
        public string stage { get; set; }
        public DateTime stage_updated { get; set; }
    }

    public class NextStage
    {
        public string stage { get; set; }
        public DateTime stage_start_timestamp { get; set; }
    }

    public class Status
    {
        public Capetown capetown { get; set; }
        public Eskom eskom { get; set; }
    }
}

//{
//    'status': {
//        'capetown': {
//            'name': 'Cape Town',
//            'next_stages': [
//                {
//                      'stage': '1',
//                    'stage_start_timestamp': '2022-08-08T17:00:00+02:00'
//                },
//                {
//                'stage': '0',
//                    'stage_start_timestamp': '2022-08-08T22:00:00+02:00'
//                }
//            ],
//            'stage': '0',
//            'stage_updated': '2022-08-08T00:08:16.837063+02:00'
//        },
//        'eskom': {
//            'name': 'National',
//            'next_stages': [
//                {
//                'stage': '2',
//                    'stage_start_timestamp': '2022-08-08T16:00:00+02:00'
//                },
//                {
//                'stage': '0',
//                    'stage_start_timestamp': '2022-08-09T00:00:00+02:00'
//                }
//            ],
//            'stage': '0',
//            'stage_updated': '2022-08-08T16:12:53.725852+02:00'
//        }
//    }
//}