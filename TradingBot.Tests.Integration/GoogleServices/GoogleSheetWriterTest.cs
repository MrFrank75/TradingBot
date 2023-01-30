using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBot.BinanceServices;
using TradingBot.Models;
using TradingBot.Tests.Integration.XUnitUtilities;
using Xunit.Abstractions;

namespace TradingBot.Tests.Integration.GoogleServices
{
    public class GoogleSheetWriterTest
    {
        private readonly ILogger<GoogleSheetWriterTest> _logger;

        public GoogleSheetWriterTest(ITestOutputHelper testOutputHelper)
        {
            _logger = XUnitLogger.CreateLogger<GoogleSheetWriterTest>(testOutputHelper);
        }

        [Fact]
        public async Task CanWriteRowInGoogleSheet()
        {
            var sut = new TradingBot.GoogleServices.GoogleSheetWriter();
            await  sut.WriteCsvRowsIntoSheet("1PMYJvYX8ryckzLiH8xkDdrrYDi7Q64GbPNsMnLDATyE", new List<OrderBookCSVSnapshotEntry>(),
                1);
        }
        [Fact]
        public async Task CanWriteCsvRowInGoogleSheet()
        {
            var sut = new TradingBot.GoogleServices.GoogleSheetWriter();
            await sut.WriteCsvRowsIntoSheet("1PMYJvYX8ryckzLiH8xkDdrrYDi7Q64GbPNsMnLDATyE", new List<OrderBookCSVSnapshotEntry>() { 
                new OrderBookCSVSnapshotEntry
                {
                    GenerationUtcDateTime = "1",
                    PriceLevel = 2,
                    Quantity = 3
                }
                },
            1);
        }

    }
}
