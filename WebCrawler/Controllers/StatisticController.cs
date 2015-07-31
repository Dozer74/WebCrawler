using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using WebCrawler.DAL;
using WebCrawler.Models;

namespace WebCrawler.Controllers
{
    public class StatisticController : Controller
    {
        private const int RecordsOnChart = 90;
        private const int RecordsInTable = 150;
        private readonly DataProvider dataProvider;

        public StatisticController()
        {
            dataProvider = new DataProvider(new EFDataProvider(), RecordsOnChart, RecordsInTable);
        }

        public ActionResult GetAll()
        {
            var records = dataProvider.GetRecordsForInfoTable();

            if (!records.Any())
            {
                return EmptyModelView();
            }

            return ParseStatistic(records.ToArray(), new EFGroupInfoProvider());
        }

        private ActionResult ParseStatistic(DataModel[] records, IGroupInfoProvider infoProvider)
        {
            records = Precount(records);

            var model = new StatisticModel
            {
                GroupName = infoProvider.GetSavedGroupName(),
                GroupUrl = infoProvider.GetSavedGroupUrl(),
                RecordsCount = dataProvider.GetRecordsCount(),
                LastUpdateTime = records.Max(st => st.UpdatingTime),
                Records = records.ToArray()
            };

            return View("GetAll", model);
        }

        private DataModel[] Precount(DataModel[] records)
        {
            for (var i = 0; i < records.Length; i++)
            {
                if (i < records.Length - 1)
                    records[i].Delta = records[i].MembersCount - records[i + 1].MembersCount;
                records[i].UpdatingTime = ConvertToLocalTime(records[i].UpdatingTime);
            }
            return records;
        }

        private ActionResult EmptyModelView()
        {
            return View("EmptyModel");
        }

        public JsonResult BuildChart()
        {
            var records = dataProvider.GetRecordsForChart();

            var data = records.Select(r => new
            {
                Date =
                    ConvertToLocalTime(r.UpdatingTime)
                        .ToString("dd MMMM в HH:mm", CultureInfo.CreateSpecificCulture("ru-RU")),
                r.MembersCount
            });

            var max = records.Max(r => r.MembersCount);
            var min = records.Min(r => r.MembersCount);
            var delta = Math.Max(max - min, 50);

            var chartData = new {Records = data, Min = min - delta/3, Max = max + delta/3};

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        private DateTime ConvertToLocalTime(DateTime date)
        {
            return date.AddHours(5); //Время на сервере отличается от UTC+5
        }
    }
}