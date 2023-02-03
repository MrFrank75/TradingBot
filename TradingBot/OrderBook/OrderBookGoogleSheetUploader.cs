using TradingBot.GoogleServices;

namespace TradingBot.OrderBook
{
    public class OrderBookGoogleSheetUploader
    {
        private const int MS_IN_A_SECOND = 1000;
        private readonly ILogger<OrderBookGoogleSheetUploader> _logger;
        private readonly IPriceRangeQuantizer _priceRangeQuantizer;
        private readonly IGoogleSheetWriter _gsw;


        public OrderBookGoogleSheetUploader(ILogger<OrderBookGoogleSheetUploader> logger,IPriceRangeQuantizer priceRangeQuantizer, IGoogleSheetWriter googleSheetWriter)
        {
            _logger = logger;
            _priceRangeQuantizer = priceRangeQuantizer;
            _gsw = googleSheetWriter;
        }

        public async Task ContinuoslyUpdateOrderBookInGoogleSheetByColumn(CancellationToken cancellationToken, int intervalSecondsBetweenGenerations,string googleSheetId, List<OrderBookEntry> Entries,double lowPriceRange, double highPriceRange, double priceGranularity)
        {
            int startingColumn = 2;
            bool headerCreated = false;

            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    if (headerCreated== false)
                    {
                        _gsw.WriteReferencePriceLevelRanges(googleSheetId, lowPriceRange, highPriceRange, priceGranularity);
                        headerCreated= true;
                    }

                    string generationUtcDateTime = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-ff");
                    List<OrderBookCSVSnapshotEntry> orderBookCSVSnapshotEntries = Entries.AsParallel().Select(item => CreateOrderBookCSVSnapshotEntry(item, generationUtcDateTime)).ToList();
                    _gsw.WritePriceLevelsRangeWithQuantitiesIntoSheetByColumn(googleSheetId, orderBookCSVSnapshotEntries, lowPriceRange,highPriceRange,priceGranularity, startingColumn);
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

        public async Task ContinuoslyUpdateOrderBookInGoogleSheetByRow(CancellationToken cancellationToken,string googleSheetId, int intervalSecondsBetweenGenerations, double lowPriceRange, double highPriceRange, double priceGranularity, int lowVolumePercToFilterOut, IOrderBook orderBook)
        {
            int startingRow = 2;
            var maxLevel = _priceRangeQuantizer.GetMaxLevel(lowPriceRange, highPriceRange, priceGranularity);


            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    if (orderBook.TickerInfo == null)
                    {
                        _logger.LogInformation("Ticker info not loaded yet. This update will be skipped");
                        await Task.Delay(1000);
                        continue;
                    }
                    string secondsFromBeginningOfTheDay = Convert.ToInt64((DateTime.UtcNow - new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day)).TotalSeconds).ToString();
                    
                    List<OrderBookCSVSnapshotEntry> orderBookCSVSnapshotEntries = orderBook.Entries.AsParallel().Select(item => CreateOrderBookCSVSnapshotEntry(item, secondsFromBeginningOfTheDay)).ToList();
                    if (orderBookCSVSnapshotEntries.Any()) {
                        _logger.LogInformation($"Updating Google Sheet with rowsId {secondsFromBeginningOfTheDay} - Ticker {orderBook.TickerInfo.Ticker} - Ticker Price {orderBook.TickerInfo.Price}");
                        int writtenRows = _gsw.WritePriceLevelsRangeWithQuantitiesIntoSheetByRow(googleSheetId, orderBookCSVSnapshotEntries, lowPriceRange, highPriceRange, priceGranularity, lowVolumePercToFilterOut, orderBook.TickerInfo.Price, startingRow);
                        startingRow += (writtenRows + 1);
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
