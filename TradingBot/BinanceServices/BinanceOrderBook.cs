using Newtonsoft.Json;
using System.Text;

namespace TradingBot.BinanceServices
{
    public class BinanceOrderBook
    {
        private const int PriceGranularity = 50;
        private readonly ILogger<BinanceOrderBook> _logger;
        private readonly IBinanceConnectorWrapper _binanceConnectorWrapper;
        private readonly IOrderBookBuilder _orderBookBuilder;
        private List<OrderBookEntry> _entries;

        public List<OrderBookEntry> Entries { get => _entries; }

        public BinanceOrderBook(ILogger<BinanceOrderBook> logger, IBinanceConnectorWrapper binanceConnectorWrapper, IOrderBookBuilder orderBookBuilder)
        {
            _logger = logger;
            _binanceConnectorWrapper = binanceConnectorWrapper;
            _orderBookBuilder = orderBookBuilder;
            _entries = new List<OrderBookEntry>();
        }

        public async Task Build(string symbol, CancellationToken cancellationToken)
        {
            try
            {
                //start listening to the stream
                var stream = $"{symbol.ToLowerInvariant()}@depth@500ms";
                Task<int> taskStreamListening = _binanceConnectorWrapper.ListenToOrderBookDepthStream(stream, cancellationToken);

                //get the initial snapshot
                _logger.LogInformation("Fetching initial snapshot");
                PayloadModels.API.OrderBookAPISnapshot? initialSnapshot = await _binanceConnectorWrapper.LoadInitialOrderBookSnapshot(symbol.ToUpperInvariant());
                if (initialSnapshot == null)
                {
                    throw new Exception("the returned initial Order book snapshot was null. This was unexpected.");
                }
                _logger.LogInformation($"Initial Snapshot last update ID:{initialSnapshot.LastUpdateId}");
                _orderBookBuilder.BuildFromSnapshot(initialSnapshot, Entries, PriceGranularity);

                //start the periodic generation of the CSV
                Task taskGenerateOrderBookSnapshot = ContinuoslyGenerateOrderBookSnapshot(cancellationToken, 5);

                //keep adding up order book entries as they come, until cancellation is requested
                while (cancellationToken.IsCancellationRequested == false)
                {
                    await Task.Delay(5000);  //give some rest to the CPU

                    //TODO: this zero needs to be changed, we are taking the initial snapshot from the wrong API point
                    _orderBookBuilder.BuildFromBidAskEntriesStream(_binanceConnectorWrapper.OrderBookDiffMessages, Entries, 0, PriceGranularity);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception caught. Message:{ex.Message}");
                throw;
            }
            _logger.LogInformation("Populate has been correctly terminated");
        }

        private async Task ContinuoslyGenerateOrderBookSnapshot(CancellationToken cancellationToken, int intervalSecondsBetweenGenerations)
        {
            string csvDirectory = $"{Environment.CurrentDirectory}\\{DateTime.Now.ToString("yyyyMMdd-hhmmss")}";
            Directory.CreateDirectory(csvDirectory);

            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    string generationId = Guid.NewGuid().ToString();
                    List<OrderBookCSVSnapshotEntry> orderBookCSVSnapshotEntries = Entries.AsParallel().Select(item => CreateOrderBookCSVSnapshotEntry(item, generationId)).ToList();

                    List<string> csv = new List<string>();
                    foreach (var item in orderBookCSVSnapshotEntries)
                    {
                        csv.Add($"{item.GenerationId};{item.PriceLevel};{item.Quantity.ToString("0.00")}");
                    }
                    File.AppendAllLines($"{csvDirectory}\\snapshot.csv", csv);

                    _logger.LogInformation($"CSV Snapshot generated. Contains {orderBookCSVSnapshotEntries.Count} entries");
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
