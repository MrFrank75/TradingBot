using TradingBot.BinanceServices;
using TradingBot.GoogleServices;
using TradingBot.Models;

namespace TradingBot.CommonServices
{
    public class OrderBookGoogleSheetUploader
    {
        private const int MS_IN_A_SECOND = 1000;
        private readonly ILogger<OrderBookGoogleSheetUploader> _logger;
        private readonly string _googleSheetId;
        private readonly IPriceRangeQuantizer _priceRangeQuantizer;
        private readonly IGoogleSheetWriter _gsw;

        public OrderBookGoogleSheetUploader(ILogger<OrderBookGoogleSheetUploader> logger, string googleSheetId, IPriceRangeQuantizer priceRangeQuantizer, IGoogleSheetWriter googleSheetWriter)
        {
            _logger = logger;
            _googleSheetId = googleSheetId;
            _priceRangeQuantizer = priceRangeQuantizer;
            _gsw = googleSheetWriter;

        }

        public async Task ContinuoslyUpdateOrderBookInGoogleSheetByColumn(CancellationToken cancellationToken, int intervalSecondsBetweenGenerations, List<OrderBookEntry> Entries, double lowPriceRange, double highPriceRange, double priceGranularity)
        {
            int startingColumn = 2;
            bool headerCreated = false;

            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    if (headerCreated== false)
                    {
                        _gsw.WriteReferencePriceLevelRanges(_googleSheetId, lowPriceRange, highPriceRange, priceGranularity);
                        headerCreated= true;
                    }

                    string generationUtcDateTime = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-ff");
                    List<OrderBookCSVSnapshotEntry> orderBookCSVSnapshotEntries = Entries.AsParallel().Select(item => CreateOrderBookCSVSnapshotEntry(item, generationUtcDateTime)).ToList();
                    _gsw.WritePriceLevelsRangeWithQuantitiesIntoSheetByColumn(_googleSheetId, orderBookCSVSnapshotEntries, lowPriceRange,highPriceRange,priceGranularity, startingColumn);
                    startingColumn++;

                    await Task.Delay(intervalSecondsBetweenGenerations * MS_IN_A_SECOND);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception caught. Message:{ex.Message}");
                    throw;
                }
            }
        }

        public async Task ContinuoslyUpdateOrderBookInGoogleSheetByRow(CancellationToken cancellationToken, int intervalSecondsBetweenGenerations, List<OrderBookEntry> Entries, double lowPriceRange, double highPriceRange, double priceGranularity)
        {
            int startingRow = 2;
            var maxLevel = _priceRangeQuantizer.GetMaxLevel(lowPriceRange, highPriceRange, priceGranularity);

            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    string generationUtcDateTime = DateTime.UtcNow.ToString("HHmmssff");
                    List<OrderBookCSVSnapshotEntry> orderBookCSVSnapshotEntries = Entries.AsParallel().Select(item => CreateOrderBookCSVSnapshotEntry(item, generationUtcDateTime)).ToList();
                    if (orderBookCSVSnapshotEntries.Any()) {
                        _gsw.WritePriceLevelsRangeWithQuantitiesIntoSheetByRow(_googleSheetId, orderBookCSVSnapshotEntries, lowPriceRange, highPriceRange, priceGranularity, startingRow);
                        startingRow += (maxLevel + 1);
                    }

                    await Task.Delay(intervalSecondsBetweenGenerations * MS_IN_A_SECOND);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception caught. Message:{ex.Message}");
                    throw;
                }
            }
        }

        private OrderBookCSVSnapshotEntry CreateOrderBookCSVSnapshotEntry(OrderBookEntry item, string generationUtcDateTime)
        {
            return new OrderBookCSVSnapshotEntry
            {
                GenerationUtcDateTime = generationUtcDateTime,
                PriceLevel = item.PriceLevel,
                Quantity = item.Quantity
            };
        }

    }
}
