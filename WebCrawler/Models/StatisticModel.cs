using System;
using System.Collections.Generic;

namespace WebCrawler.Models
{
    public class StatisticModel
    {
        public string GroupName { get; set; }
        public int RecordsCount { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string GroupUrl { get; set; }
        public DataModel[] Records { get; set; }  
    }
}