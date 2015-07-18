using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using WebCrawler.Models;

namespace WebCrawler.Controllers
{
    public class HomeController : Controller
    {
        private readonly StatisticDbEntities entities = new StatisticDbEntities();
        // GET: Home
        public ActionResult Index()
        {
            var stat = entities.Statistic.ToList();
            var dates = stat.Select(s => s.UpdatingTime.ToLongDateString()).ToArray();
            var values = stat.Select(s => s.MembersCount).ToArray();

            var yMin = values.Min()-1;
            var yMax = values.Max()+1;

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
                LastUpdateTime = stat.Max(s => s.UpdatingTime)
            };

            return View(model);
        }
    }
}