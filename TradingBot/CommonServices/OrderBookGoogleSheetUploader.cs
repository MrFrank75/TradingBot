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
        private readonly GoogleSheetWriter _gsw;

        public OrderBookGoogleSheetUploader(ILogger<OrderBookGoogleSheetUploader> logger, string googleSheetId)
        {
            _logger = logger;
            _googleSheetId = googleSheetId;
            _gsw = new GoogleServices.GoogleSheetWriter();

        }

        public async Task ContinuoslyUpdateOrderBookInGoogleSheet(CancellationToken cancellationToken, int intervalSecondsBetweenGenerations, List<OrderBookEntry> Entries)
        {
            int startingRow = 1;
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    string generationUtcDateTime = DateTime.UtcNow.ToString("yyyyMMddHHmmssff");
                    List<OrderBookCSVSnapshotEntry> orderBookCSVSnapshotEntries = Entries.AsParallel().Select(item => CreateOrderBookCSVSnapshotEntry(item, generationUtcDateTime)).ToList();
                    await _gsw.WriteCsvRowsIntoSheet(_googleSheetId, orderBookCSVSnapshotEntries, startingRow);
                    startingRow += orderBookCSVSnapshotEntries.Count;

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
