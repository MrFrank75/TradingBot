using TradingBot.BinanceServices;
using TradingBot.GoogleServices;
using TradingBot.Models;

namespace TradingBot.CommonServices
{
    public class OrderBookGoogleSheetUploader
    {
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
                    string generationId = Guid.NewGuid().ToString();
                    List<OrderBookCSVSnapshotEntry> orderBookCSVSnapshotEntries = Entries.AsParallel().Select(item => CreateOrderBookCSVSnapshotEntry(item, generationId)).ToList();
                    await _gsw.WriteCsvRowsIntoSheet(_googleSheetId, orderBookCSVSnapshotEntries, startingRow);
                    startingRow += orderBookCSVSnapshotEntries.Count;

                    await Task.Delay(intervalSecondsBetweenGenerations * 1000);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception caught. Message:{ex.Message}");
                    throw;
                }
            }
        }

        private OrderBookCSVSnapshotEntry CreateOrderBookCSVSnapshotEntry(OrderBookEntry item, string generationId)
        {
            return new OrderBookCSVSnapshotEntry
            {
                GenerationId = generationId,
                PriceLevel = item.PriceLevel,
                Quantity = item.Quantity
            };
        }

    }
}
