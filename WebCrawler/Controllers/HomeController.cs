using System.Collections.Generic;
using Ninject;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using WebCrawler.Models;

namespace WebCrawler.Controllers
{
    public class HomeController : Controller
    {
        readonly IKernel kernel = new StandardKernel(new SettingsNinjectModule());

        //private readonly StatisticEntities entities = new StatisticEntities();
        // GET: Home
        public ActionResult Index()
        {
            var dataProvaire = kernel.Get<IDatabaseProvider>();
            var groupInfoProvider = kernel.Get<IGroupInfoProvider>();

            var records = dataProvaire.GetAllRecords();

            if (records.Count() == 0)
            {
                return EmptyModelView();
            }
            else
            {
                return ParseStatistic(records, groupInfoProvider);
            }

            /*var stat = entities.Statistic.ToList();
            var dates = stat.Select(st => st.UpdatingTime.ToLongDateString()+" в "+st.UpdatingTime.ToShortTimeString()).ToArray();
            var values = stat.Select(st => st.MembersCount).ToArray();

            if (values.Count() == 0)// В базе нет данных
            {
                var emptyModel = new StatisticModel
                {
                    GroupName = "База данных пуста!",
                    GroupUrl = "База данных пуста!",
                    RecordsCount = 0
                };
                return View(emptyModel);
            }


            var yMin = values.Min()-50;
            var yMax = values.Max()+50;

            new Chart(800, dates.Length*100)
                .AddSeries(chartType: "bar",
                    xValue: dates,
                    yValues: values)
                .SetYAxis(null, yMin, yMax)
                .Save("~/Images/chart.png", "png");

            var model = new StatisticModel
            {
                GroupName = entities.GroupInfo.FirstOrDefault()?.GroupName,
                GroupUrl = entities.GroupInfo.FirstOrDefault()?.GroupUrl,
                RecordsCount = dates.Length,
                LastUpdateTime = stat.Max(st => st.UpdatingTime)
            };

            return View(model);*/
        }

        private ActionResult ParseStatistic(IEnumerable<DataModel> records, IGroupInfoProvider infoProvider)
        {
            var model = new StatisticModel
            {
                GroupName = infoProvider.GetSavedGroupName(),
                GroupUrl = infoProvider.GetSavedGroupUrl(),
                RecordsCount = records.Count(),
                LastUpdateTime = records.Max(st => st.UpdatingTime),
                Records = records
            };

            /*var yMin = records.Min(r => r.MembersCount) - 50;
            var yMax = records.Max(r => r.MembersCount) + 50;

            var chart = new Chart(800, records.Count() * 100)
                .AddSeries(chartType: "bar",
                    xValue: records.Select(r => r.UpdatingTime),
                    yValues: records.Select(r => r.MembersCount))
                .SetYAxis(null, yMin, yMax);

            chart.Save("~/Images/chart.png", "png");*/

            return View("Index", model);
        }

        private ActionResult EmptyModelView()
        {
            return View("EmptyModel");
        }
    }
}