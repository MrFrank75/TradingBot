﻿using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.Models;
using TradingBot.OrderBook;

namespace TradingBot.BinanceServices
{
    public class BinanceOrderBook : IOrderBook
    {
        private const int PriceGranularity = 50;
        private const int MillisecondsDelay = 5000;
        private readonly ILogger<BinanceOrderBook> _logger;
        private readonly IBinanceOrderBookConnector _binanceConnectorWrapper;
        private readonly IOrderBookBuilder _orderBookBuilder;
        private List<OrderBookEntry> _entries;
        private Symbol _tickerInfo;

        public List<OrderBookEntry> Entries { get => _entries; }
        public Symbol? TickerInfo { get => _tickerInfo; }

        public BinanceOrderBook(ILogger<BinanceOrderBook> logger, IBinanceOrderBookConnector binanceConnectorWrapper, IOrderBookBuilder orderBookBuilder)
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
                
                //provides initial info about the ticker
                BinanceSymbol binanceSymbol = await _binanceConnectorWrapper.GetSymbolPriceTicker(symbol.ToUpperInvariant());
                _tickerInfo = binanceSymbol.ToSymbol();

                //start listening to the stream. We should be waiting that the first elements arrive before moving forward
                var stream = $"{symbol.ToLowerInvariant()}@depth@500ms";
                Task<int> taskStreamListening = _binanceConnectorWrapper.ListenToOrderBookDepthStream(stream, cancellationToken);

                //TODO: change this, it is a poor man's delay for the order book stream to start
                await Task.Delay(5000);

                _logger.LogInformation("Fetching initial snapshot");
                PayloadModels.API.OrderBookAPISnapshot? initialSnapshot = await _binanceConnectorWrapper.LoadInitialOrderBookSnapshot(symbol.ToUpperInvariant());
                if (initialSnapshot == null)
                {
                    throw new Exception("the returned initial Order book snapshot was null. This was unexpected.");
                }

                _logger.LogInformation($"Initial Snapshot last update ID:{initialSnapshot.LastUpdateId}");
                _orderBookBuilder.BuildFromSnapshot(initialSnapshot, Entries, PriceGranularity);

                //keep adding up order book entries as they come, until cancellation is requested
                while (cancellationToken.IsCancellationRequested == false)
                {
                    BinanceSymbol updatedSymbolInfo = await _binanceConnectorWrapper.GetSymbolPriceTicker(symbol.ToUpperInvariant());
                    _tickerInfo = updatedSymbolInfo.ToSymbol();
                    _orderBookBuilder.BuildFromBidAskEntriesStream(_binanceConnectorWrapper.OrderBookDiffMessages, Entries, initialSnapshot.LastUpdateId, PriceGranularity);
                    await Task.Delay(MillisecondsDelay);  //give some rest to the CPU
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception caught. Message:{ex.Message}");
                throw;
            }
            _logger.LogInformation("Populate has been correctly terminated");
        }
    }
}
