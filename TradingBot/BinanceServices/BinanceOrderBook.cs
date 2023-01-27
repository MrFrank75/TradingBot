using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;

namespace TradingBot.BinanceServices
{
    public class BinanceOrderBook
    {
        private readonly ILogger<BinanceOrderBook> _logger;
        private readonly IBinanceConnectorWrapper _binanceConnectorWrapper;

        public List<OrderBookEntry> Entries { get; private set; }

        public BinanceOrderBook(ILogger<BinanceOrderBook> logger, IBinanceConnectorWrapper binanceConnectorWrapper)
        {
            this._logger = logger;
            this._binanceConnectorWrapper = binanceConnectorWrapper;
        }

        public async Task Populate(string symbol, CancellationToken cancellationToken) {
            try
            {
                //start listening to the stream
                var stream = $"{symbol.ToLowerInvariant()}@depth@500ms";
                Task<int> taskStreamListening = _binanceConnectorWrapper.ListenToOrderBookDepthStream(stream, cancellationToken);

                //get the initial snapshot
                PayloadModels.API.OrderBookAPISnapshot? initialSnapshot = await _binanceConnectorWrapper.LoadInitialOrderBookSnapshot(symbol.ToUpperInvariant());
                if (initialSnapshot == null)
                {
                    throw new Exception("the returned initial Order book snapshot was null. This was unexpected.");
                }

                PopulateEntryListFromInitialOrderBookSnapshot(initialSnapshot, Entries);

                //keep adding up order book entries as they come, until cancellation is requested
                while (cancellationToken.IsCancellationRequested == false)
                {
                    Task.Delay(10).Wait();  //give some rest to the CPU
                    PopulateEntryListWithCurrentlyAvailableData(_binanceConnectorWrapper.OrderBookMessages, Entries);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            _logger.LogInformation("Populate has been correctly terminated");
        }

        private void PopulateEntryListWithCurrentlyAvailableData(ConcurrentQueue<string> orderBookMessages, List<OrderBookEntry> orderBookToUpdate)
        {
            return;
        }

        private void PopulateEntryListFromInitialOrderBookSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> orderBookToUpdate)
        {
            return;
        }
    }
}
