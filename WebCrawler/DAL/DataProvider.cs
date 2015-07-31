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
        private readonly int maxRecords—ountOnChart;

        public DataProvider(IDatabaseProvider databaseProvider, int maxRecords—ountOnChart, int maxRecordsCountInTable)
        {
            this.databaseProvider = databaseProvider;
            this.maxRecords—ountOnChart = maxRecords—ountOnChart;
            this.maxRecordsCountInTable = maxRecordsCountInTable;
        }

        public int GetRecordsCount()
        {
            return databaseProvider.GetAllRecords().Count();
        }

        public IEnumerable<DataModel> GetRecordsForChart()
        {
            return databaseProvider.GetAllRecords().Reverse().Take(maxRecords—ountOnChart);
        }

        public IEnumerable<DataModel> GetRecordsForInfoTable()
        {
            return databaseProvider.GetAllRecords().Reverse().Take(maxRecordsCountInTable);
        }
    }
}