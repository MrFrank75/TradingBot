using System.Collections.Concurrent;
using System.Collections.Generic;
using TradingBot.BinanceServices.PayloadModels.API;

namespace TradingBot.BinanceServices
{
    public class BinanceOrderBook
    {
        private const int PriceGranularity = 100;
        private readonly ILogger<BinanceOrderBook> _logger;
        private readonly IBinanceConnectorWrapper _binanceConnectorWrapper;
        private readonly IOrderBookConverter _orderBookConverter;

        public List<OrderBookEntry> Entries { get; private set; }

        public BinanceOrderBook(ILogger<BinanceOrderBook> logger, IBinanceConnectorWrapper binanceConnectorWrapper, IOrderBookConverter orderBookConverter)
        {
            _logger = logger;
            _binanceConnectorWrapper = binanceConnectorWrapper;
            _orderBookConverter = orderBookConverter;
        }

        public async Task Build(string symbol, CancellationToken cancellationToken) {
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

                _orderBookConverter.BuildFromSnapshot(initialSnapshot, Entries, PriceGranularity);

                //keep adding up order book entries as they come, until cancellation is requested
                while (cancellationToken.IsCancellationRequested == false)
                {
                    Task.Delay(10).Wait();  //give some rest to the CPU
                    _orderBookConverter.BuildFromBidAskEntriesStream(_binanceConnectorWrapper.OrderBookDiffMessages, Entries, initialSnapshot.LastUpdateId, PriceGranularity);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            _logger.LogInformation("Populate has been correctly terminated");
        }

    }
}
