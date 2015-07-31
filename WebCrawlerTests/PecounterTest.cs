using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WebCrawler.Controllers;
using WebCrawler.Models;

namespace WebCrawlerTests
{
    class PecounterTest
    {
     

        [Test]
        public void CountDeltaBetweenRecords()
        {
            var input = new[]
            {
                new DataModel {MembersCount = 10},
                new DataModel {MembersCount = 5},
                new DataModel {MembersCount = 7},
                new DataModel {MembersCount = 1}
            };

            var expected = new[] {5,-2,6,0};

            var controller = new StatisticController();
            var actual = controller.Precount(input).Select(a =>a.Delta);

            CollectionAssert.AreEqual(expected,actual);
        }

        [Test]
        public void ConvertTimeToLocal()
        {
            var input = new[]
            {
                new DataModel {MembersCount = 100, UpdatingTime = new DateTime(2015, 07, 31, 12, 10, 10)}
            };

            var controller = new StatisticController();
            var actual = controller.Precount(input);

            var expected = new[]
            {
                new DataModel {MembersCount = 100, UpdatingTime = new DateTime(2015, 07, 31, 17, 10, 10)}
            };
            Assert.AreEqual(expected,actual);
        }
    }
}
