using System.Collections.Generic;
using System.Linq;

namespace WebCrawler.Models
{
    class EFDataProvider : IDatabaseProvider
    {
        private readonly StatisticEntities db;

        public EFDataProvider()
        {
            db = new StatisticEntities();
        }

        public void AddRecord(DataModel model)
        {
            db.Statistic.Add(new Statistic
            {
                Id = 0,
                MembersCount = model.MembersCount,
                UpdatingTime = model.UpdatingTime
            });
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void Truncate()
        {
            db.Statistic.RemoveRange(db.Statistic);
            db.SaveChanges();
        }

        public IEnumerable<DataModel> GetAllRecords()
        {
            return
                db.Statistic.ToList()
                    .Select(m => new DataModel {MembersCount = m.MembersCount, UpdatingTime = m.UpdatingTime});
        }
    }
}
