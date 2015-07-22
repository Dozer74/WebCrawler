using Ninject.Modules;
using WebCrawler.DAL;
using WebCrawler.Models;

namespace WebCrawler
{
    public class SettingsNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabaseProvider>().To<EFDataProvider>();
            Bind<IGroupInfoProvider>().To<EFGroupInfoProvider>();
        }
    }
}