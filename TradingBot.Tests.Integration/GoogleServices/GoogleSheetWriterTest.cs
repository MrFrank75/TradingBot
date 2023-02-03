using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBot.BinanceServices;
using TradingBot.OrderBook;
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
        public void CanWriteRowInGoogleSheet()
        {
            IPriceRangeQuantizer prq = new PriceRangeQuantizer();
            var sut = new TradingBot.GoogleServices.GoogleSheetWriter(prq);
            sut.WriteCsvRowsIntoSheet("1FAT1lvMmWDje_tdaNTygz8tTwpv2EcKFqhmTjBJ62i8", new List<OrderBookCSVSnapshotEntry>(),
                1);
        }
        [Fact]
        public void CanWriteCsvRowInGoogleSheet()
        {
            IPriceRangeQuantizer prq = new PriceRangeQuantizer();
            var sut = new TradingBot.GoogleServices.GoogleSheetWriter(prq);
            sut.WriteCsvRowsIntoSheet("1FAT1lvMmWDje_tdaNTygz8tTwpv2EcKFqhmTjBJ62i8", new List<OrderBookCSVSnapshotEntry>() { 
                new OrderBookCSVSnapshotEntry
                {
                    GenerationUtcDateTime = "1",
                    PriceLevel = 23452,
                    Quantity = 12233.213
                },
                new OrderBookCSVSnapshotEntry
                {
                    GenerationUtcDateTime = "1",
                    PriceLevel = 23452,
                    Quantity = 12233.213
                }
                },
            1);
        }


        [Fact]
        public void CanWritePriceLevelColumnsInGoogleSheet()
        {
            IPriceRangeQuantizer prq = new PriceRangeQuantizer();
            var sut = new TradingBot.GoogleServices.GoogleSheetWriter(prq);
            sut.WritePriceLevelsRangeWithQuantitiesIntoSheetByColumn("1FAT1lvMmWDje_tdaNTygz8tTwpv2EcKFqhmTjBJ62i8",
                new List<OrderBookCSVSnapshotEntry>() {
                    new OrderBookCSVSnapshotEntry
                    {
                        GenerationUtcDateTime = "1",
                        PriceLevel = 23452,
                        Quantity = 12233.213
                    },
                    new OrderBookCSVSnapshotEntry
                    {
                        GenerationUtcDateTime = "1",
                        PriceLevel = 23852,
                        Quantity = 12233.213
                    }
                    },
                20000,
                24000,
                1000,
                2);
        }

        [Fact]
        public void CanWritePriceLevelsInRowsInGoogleSheet()
        {
            IPriceRangeQuantizer prq = new PriceRangeQuantizer();
            var sut = new TradingBot.GoogleServices.GoogleSheetWriter(prq);
            sut.WritePriceLevelsRangeWithQuantitiesIntoSheetByRow("1FAT1lvMmWDje_tdaNTygz8tTwpv2EcKFqhmTjBJ62i8",
                new List<OrderBookCSVSnapshotEntry>() {
                    new OrderBookCSVSnapshotEntry
                    {
                        GenerationUtcDateTime = "1",
                        PriceLevel = 23000,
                        Quantity = 1000
                    },
                    new OrderBookCSVSnapshotEntry
                    {
                        GenerationUtcDateTime = "1",
                        PriceLevel = 23200,
                        Quantity = 1000
                    }
                    },
                20000,
                24000,
                1000,
                21545,
                1);
        }

        [Fact]
        public void CanWriteReferencePriceLevelsInGoogleSheet()
        {
            IPriceRangeQuantizer prq = new PriceRangeQuantizer();
            var sut = new TradingBot.GoogleServices.GoogleSheetWriter(prq);
            sut.WriteReferencePriceLevelRanges("1FAT1lvMmWDje_tdaNTygz8tTwpv2EcKFqhmTjBJ62i8",
                20000,
                24000,
                100);
        }

    }
}
