using Moq;
using NUnit.Framework;
using WebCrawler.Controllers;
using WebCrawler.DAL;
using WebCrawler.Models;

namespace WebCrawlerTests
{
    [TestFixture]
    public class DataProviderShould
    {
        private Mock<IDatabaseProvider> dataProviderMock;

        [SetUp]
        public void Init()
        {
            dataProviderMock = new Mock<IDatabaseProvider>();
        }

        [Test]
        public void ReturnLatestRecords()
        {
            var inputData = new DataModel[100];
            for (var i = 0; i < 100; i++)
            {
                inputData[i] = new DataModel {MembersCount = i + 1};
            }
            dataProviderMock.Setup(m => m.GetAllRecords()).Returns(inputData);

            var expectedTable = new[]
            {
                new DataModel {MembersCount = 100},
                new DataModel {MembersCount = 99},
                new DataModel {MembersCount = 98},
                new DataModel {MembersCount = 97},
                new DataModel {MembersCount = 96}
            };

            var expectedChart = new[]
            {
                new DataModel {MembersCount = 100},
                new DataModel {MembersCount = 99},
                new DataModel {MembersCount = 98}
            };

            var provider = new DataProvider(dataProviderMock.Object, 3, 5);
            var count = provider.GetRecordsCount();
            var recordsForChart = provider.GetRecordsForChart();
            var recordsForTable = provider.GetRecordsForInfoTable();

            Assert.AreEqual(count, 100);
            CollectionAssert.AreEqual(expectedTable, recordsForTable);
            CollectionAssert.AreEqual(expectedChart, recordsForChart);
        }

        [Test]
        public void ReverseRecords()
        {
            dataProviderMock.Setup(m => m.GetAllRecords()).Returns(new[]
            {
                new DataModel {MembersCount = 1},
                new DataModel {MembersCount = 2},
                new DataModel {MembersCount = 3}
            });

            var expected = new[]
            {
                new DataModel {MembersCount = 3},
                new DataModel {MembersCount = 2},
                new DataModel {MembersCount = 1}
            };

            var provider = new DataProvider(dataProviderMock.Object, 10, 10);
            var count = provider.GetRecordsCount();
            var recordsForChart = provider.GetRecordsForChart();
            var recordsForTable = provider.GetRecordsForInfoTable();

            Assert.AreEqual(count, 3);
            CollectionAssert.AreEqual(expected, recordsForTable);
            CollectionAssert.AreEqual(expected, recordsForChart);
        }

        [Test]
        public void WorkWithEmptyBase()
        {
            dataProviderMock.SetupAllProperties();

            var provider = new DataProvider(dataProviderMock.Object, 10, 10);
            var count = provider.GetRecordsCount();
            var recordsForChart = provider.GetRecordsForChart();
            var recordsForTable = provider.GetRecordsForInfoTable();

            Assert.AreEqual(count, 0);
            CollectionAssert.IsEmpty(recordsForTable);
            CollectionAssert.IsEmpty(recordsForChart);
        }
    }
}