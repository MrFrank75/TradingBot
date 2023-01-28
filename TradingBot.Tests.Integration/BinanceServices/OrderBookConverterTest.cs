using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TradingBot.BinanceServices;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;
using TradingBot.Tests.Integration.XUnitUtilities;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TradingBot.Tests.Integration.BinanceServices
{
    public class OrderBookConverterTest
    {
        private readonly ILogger<OrderBookConverter> _logger;

        public OrderBookConverterTest(ITestOutputHelper testOutputHelper)
        {
            _logger = XUnitLogger.CreateLogger<OrderBookConverter>(testOutputHelper);
        }

        [Fact]
        public async Task CanPopulateOrderBookFromEntriesWithGranularity100()
        {
            var sut = new OrderBookConverter();
            var orderBook = new List<OrderBookEntry>();
            ConcurrentQueue<DiffBookDepthStream> testDataOrderBookEntries = GetTestDataDiffBookEntries();

            sut.PopulateFromBidAskEntries(testDataOrderBookEntries, orderBook);
            var sumOfQuantity = orderBook.Sum(x => x.Quantity);

            //ASSERT
            Assert.True(Math.Round(sumOfQuantity) == 422);

        }


        [Fact]
        public async Task CanConvertInitialSnapshotWithGranularity100() { 
            var sut = new OrderBookConverter();
            var orderBook = new List<OrderBookEntry>();
            OrderBookAPISnapshot testDataSnapshot = GetTestDataSnapshot();

            sut.PopulateFromSnapshot(testDataSnapshot, orderBook, 100);
            var sumOfQuantity = orderBook.Sum(x => x.Quantity);

            //ASSERT
            Assert.True(Math.Round(sumOfQuantity) == 422);

        }

        [Fact]
        public async Task CanConvertInitialSnapshotWithGranularity50()
        {
            //ARRANGE
            var sut = new OrderBookConverter();
            var orderBook = new List<OrderBookEntry>();
            OrderBookAPISnapshot testDataSnapshot = GetTestDataSnapshot();

            //ACT
            sut.PopulateFromSnapshot(testDataSnapshot, orderBook, 50);
            var sumOfQuantity = orderBook.Sum(x => x.Quantity);

            //ASSERT
            Assert.True(Math.Round(sumOfQuantity) == 422);

        }

        [Fact]
        public async Task CanConvertInitialSnapshotWithGranularity1()
        {
            //ARRANGE
            var sut = new OrderBookConverter();
            var orderBook = new List<OrderBookEntry>();
            OrderBookAPISnapshot testDataSnapshot = GetTestDataSnapshot();

            //ACT
            sut.PopulateFromSnapshot(testDataSnapshot, orderBook, 1);
            var sumOfQuantity = orderBook.Sum(x => x.Quantity);

            //ASSERT
            Assert.True(Math.Round(sumOfQuantity) == 422);

        }


        private ConcurrentQueue<DiffBookDepthStream> GetTestDataDiffBookEntries()
        {
            var jsonEntries = File.ReadAllLines($"{Environment.CurrentDirectory}\\TestData\\OrderBookDiffBookDepthStreamMessages.txt");
            ConcurrentQueue<DiffBookDepthStream> messagesQueue = new ConcurrentQueue<DiffBookDepthStream>();

            var listOfEntries = jsonEntries.Select(line => JsonConvert.DeserializeObject<DiffBookDepthStream>(line)).ToList();
            listOfEntries.ForEach(se => {
                if (se != null)
                    messagesQueue.Enqueue(se);
            });

            return messagesQueue;
        }
        private OrderBookAPISnapshot GetTestDataSnapshot()
        {
            var testSnapshot = JsonConvert.DeserializeObject<OrderBookAPISnapshot>(File.ReadAllText($"{Environment.CurrentDirectory}\\TestData\\OrderBookInitialSnapshotTestData1.txt"));
            return testSnapshot;
        }
    }
}