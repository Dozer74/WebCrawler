using System;
using System.Collections.Generic;
using System.Globalization;
using Ninject;
using System.Linq;
using System.Web.Mvc;
using WebCrawler.DAL;
using WebCrawler.Models;

namespace WebCrawler.Controllers
{
    public class TestModel
    {
        public string Date;
        public int MembersCount;
    }

    public class HomeController : Controller
    {
        readonly IKernel kernel = new StandardKernel(new SettingsNinjectModule());

        //private readonly StatisticEntities entities = new StatisticEntities();
        // GET: Home
        public ActionResult Index()
        {
            var databaseProvider = kernel.Get<IDatabaseProvider>();
            var groupInfoProvider = kernel.Get<IGroupInfoProvider>();

            var records = databaseProvider.GetAllRecords();

            if (!records.Any())
            {
                return EmptyModelView();
            }

            return ParseStatistic(records.ToArray(), groupInfoProvider);
        }

        private ActionResult ParseStatistic(DataModel[] records, IGroupInfoProvider infoProvider)
        {
            for (int i = 1; i < records.Length; i++)
            {
                records[i].Delta = records[i].MembersCount - records[i - 1].MembersCount;
            }

            var model = new StatisticModel
            {
                GroupName = infoProvider.GetSavedGroupName(),
                GroupUrl = infoProvider.GetSavedGroupUrl(),
                RecordsCount = records.Length,
                LastUpdateTime = records.Max(st => st.UpdatingTime),
                Records = records.Reverse().ToArray() //последние записи должны отображаться в начале
            };
            
            return View("Index", model);
        }

        private ActionResult EmptyModelView()
        {
            return View("EmptyModel");
        }

        public JsonResult BuildChart()
        {
            var databaseProvider = kernel.Get<IDatabaseProvider>();
            var records = databaseProvider.GetAllRecords().Reverse();

            var data = records.Select(r => new
            {
                Date = r.UpdatingTime.ToString("dd MMMM в HH:mm", CultureInfo.CreateSpecificCulture("ru-RU")),
                r.MembersCount
            });//Преобразуем данные для построения графика

            var max = records.Max(r => r.MembersCount);
            var min = records.Min(r => r.MembersCount);

            var delta = Math.Max(max - min, 50);

            var chartData = new {Records = data, Min = min-delta/3, Max = max+delta/3};

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
    }
}