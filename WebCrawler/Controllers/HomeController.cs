using Ninject;
using System.Linq;
using System.Web.Mvc;
using WebCrawler.DAL;
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
    }
}