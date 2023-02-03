using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using TradingBot.BinanceServices;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;
using TradingBot.OrderBook;
using TradingBot.Tests.Integration.XUnitUtilities;
using Xunit.Abstractions;

namespace TradingBot.Tests.Integration.BinanceServices
{
    public class OrderBookBuilderTest
    {
        private readonly ILogger<OrderBookBuilder> _logger;

        public OrderBookBuilderTest(ITestOutputHelper testOutputHelper)
        {
            _logger = XUnitLogger.CreateLogger<OrderBookBuilder>(testOutputHelper);
        }

        [Fact]
        public async Task CanBuildOrderBookFromEntriesStreamWithGranularity100()
        {
            var sut = new OrderBookBuilder(_logger);
            var orderBook = new List<OrderBookEntry>();
            OrderBookAPISnapshot testDataSnapshot = GetTestDataInitialSnapshotForStreamTest();
            ConcurrentQueue<DiffBookDepthStream> testDataOrderBookEntries = GetTestDataFromFakeStream();
            
            //this should be mocked
            sut.BuildFromSnapshot(testDataSnapshot, orderBook, 100);
            
            sut.BuildFromBidAskEntriesStream(testDataOrderBookEntries, orderBook, testDataSnapshot.LastUpdateId, 100);
            var sumOfQuantity = orderBook.Sum(x => x.Quantity);

            //ASSERT
            Assert.True(Math.Round(sumOfQuantity) == 70);

        }

        [Fact]
        public async Task CanBuildInitialSnapshotWithGranularity100() { 
            var sut = new OrderBookBuilder(_logger);
            var orderBook = new List<OrderBookEntry>();
            OrderBookAPISnapshot testDataSnapshot = GetTestDataForInitialSnapshotTest();

            sut.BuildFromSnapshot(testDataSnapshot, orderBook, 100);
            var sumOfQuantity = orderBook.Sum(x => x.Quantity);

            //ASSERT
            Assert.True(Math.Round(sumOfQuantity) == 422);

        }

        [Fact]
        public async Task CanBuildInitialSnapshotWithGranularity50()
        {
            //ARRANGE
            var sut = new OrderBookBuilder(_logger);
            var orderBook = new List<OrderBookEntry>();
            OrderBookAPISnapshot testDataSnapshot = GetTestDataForInitialSnapshotTest();

            //ACT
            sut.BuildFromSnapshot(testDataSnapshot, orderBook, 50);
            var sumOfQuantity = orderBook.Sum(x => x.Quantity);

            //ASSERT
            Assert.True(Math.Round(sumOfQuantity) == 422);

        }

        [Fact]
        public async Task CanConvertInitialSnapshotWithGranularity1()
        {
            //ARRANGE
            var sut = new OrderBookBuilder(_logger);
            var orderBook = new List<OrderBookEntry>();
            OrderBookAPISnapshot testDataSnapshot = GetTestDataForInitialSnapshotTest();

            //ACT
            sut.BuildFromSnapshot(testDataSnapshot, orderBook, 1);
            var sumOfQuantity = orderBook.Sum(x => x.Quantity);

            //ASSERT
            Assert.True(Math.Round(sumOfQuantity) == 422);

        }


        private ConcurrentQueue<DiffBookDepthStream> GetTestDataFromFakeStream()
        {
            var jsonEntries = File.ReadAllLines($"{Environment.CurrentDirectory}\\TestData\\OrderBookDiffBookDepthStreamMessages.Testset2.txt");
            ConcurrentQueue<DiffBookDepthStream> messagesQueue = new ConcurrentQueue<DiffBookDepthStream>();

            var listOfEntries = jsonEntries.Select(line => JsonConvert.DeserializeObject<DiffBookDepthStream>(line)).ToList();
            listOfEntries.ForEach(se => {
                if (se != null)
                    messagesQueue.Enqueue(se);
            });

            return messagesQueue;
        }
        private OrderBookAPISnapshot GetTestDataForInitialSnapshotTest()
        {
            var testSnapshot = JsonConvert.DeserializeObject<OrderBookAPISnapshot>(File.ReadAllText($"{Environment.CurrentDirectory}\\TestData\\OrderBookInitialSnapshotTestData1.txt"));
            return testSnapshot;
        }

        private OrderBookAPISnapshot GetTestDataInitialSnapshotForStreamTest()
        {
            var testSnapshot = JsonConvert.DeserializeObject<OrderBookAPISnapshot>(File.ReadAllText($"{Environment.CurrentDirectory}\\TestData\\OrderBookInitialSnapshot.Testset2.txt"));
            return testSnapshot;
        }
    }
}