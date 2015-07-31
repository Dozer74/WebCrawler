using System.Collections.Generic;
using System.Linq;
using WebCrawler.DAL;
using WebCrawler.Models;

namespace WebCrawler.Controllers
{
    public class DataProvider
    {
        private readonly IDatabaseProvider databaseProvider;
        private readonly int maxRecordsCountInTable;
        private readonly int maxRecordscountOnChart;

        public DataProvider(IDatabaseProvider databaseProvider, int maxRecordscountOnChart, int maxRecordsCountInTable)
        {
            this.databaseProvider = databaseProvider;
            this.maxRecordscountOnChart = maxRecordscountOnChart;
            this.maxRecordsCountInTable = maxRecordsCountInTable;
        }

        public int GetRecordsCount()
        {
            return databaseProvider.GetAllRecords().Count();
        }

        public IEnumerable<DataModel> GetRecordsForChart()
        {
            return databaseProvider.GetAllRecords().Reverse().Take(maxRecordscountOnChart);

        }

        public IEnumerable<DataModel> GetRecordsForInfoTable()
        {
            return databaseProvider.GetAllRecords().Reverse().Take(maxRecordsCountInTable);
        }
    }
}