using Binance.Spot;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;

namespace TradingBot.BinanceServices
{
    public class BinanceConnectorWrapper : IBinanceConnectorWrapper
    {
        private readonly string BaseUrl;
        private readonly ILogger<IBinanceConnectorWrapper> _logger;
        private readonly FutureMarket _futureMarket;
        private ConcurrentQueue<string> messages = new ConcurrentQueue<string>();
        private ConcurrentQueue<DiffBookDepthStream> orderBookDiffMessages = new ConcurrentQueue<DiffBookDepthStream>();

        //this constructor uses as default address the production address specified in the documentation here https://binance-docs.github.io/apidocs/futures/en/#diff-book-depth-streams
        public BinanceConnectorWrapper(ILogger<BinanceConnectorWrapper> logger, FutureMarket futureMarket, string baseUrl = "wss://fstream.binance.com")
        {
            this.BaseUrl = baseUrl;
            this._logger = logger;
            this._futureMarket = futureMarket;
        }

        public ConcurrentQueue<string> Messages { get => messages; }
        public ConcurrentQueue<DiffBookDepthStream> OrderBookDiffMessages { get => orderBookDiffMessages; }

        public async Task<OrderBookAPISnapshot?> LoadInitialOrderBookSnapshot(string symbol) {
            var receivedMessage = await _futureMarket.GetOrderBookSnapshot(symbol,1000);

            var trimmedEntry = receivedMessage.Remove(receivedMessage.LastIndexOf("}") + 1);
            OrderBookAPISnapshot? orderBookEntry = JsonConvert.DeserializeObject<OrderBookAPISnapshot>(trimmedEntry);
            return orderBookEntry;    
        }


        public async Task<BinanceSymbol> GetSymbolPriceTicker(string symbol)
        {
            try
            {
                var receivedMessage = await _futureMarket.GetSymbolPriceTicker(symbol);
                BinanceSymbol? receivedSymbol = JsonConvert.DeserializeObject<BinanceSymbol>(receivedMessage);
                if (receivedSymbol == null)
                    throw new InvalidOperationException("Returned Symbol was null");
                
                return receivedSymbol;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async Task<int> ListenToOrderBookDepthStream(string stream, CancellationToken token)
        {
            try
            {
                string receivedMessage = string.Empty;
                MarketDataWebSocket dws = new MarketDataWebSocket(stream, BaseUrl);

                //1) register to the message received
                dws.OnMessageReceived(OrderBookMessageReceived, token);

                //2) connect before sending ANY request
                await dws.ConnectAsync(token);

                //3) SEND a subscription request to start receiving data
                Task result = dws.SendAsync(stream, token);
                result.Wait();
                if (result.IsCompletedSuccessfully == false)
                {
                    throw new Exception("Send failed");
                }

                while (token.IsCancellationRequested == false)
                {
                    //just keep waiting
                    await Task.Delay(1000);
                }

                await dws.DisconnectAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return 0;
            }

            return 1;
        }

        public async Task<int> ListenToSingleStream(string stream, CancellationToken token)
        {
            _logger.LogInformation("ListenToSingleStream started");
            string receivedMessage = string.Empty;
            MarketDataWebSocket dws = new MarketDataWebSocket(stream, BaseUrl);

            //1) register to the message received
            dws.OnMessageReceived(MessageReceived, token);

            //2) connect before sending ANY request
            await dws.ConnectAsync(token);

            //3) SEND a subscription request to start receiving data
            Task result = dws.SendAsync("emptyParam", token);
            result.Wait();
            if (result.IsCompletedSuccessfully == false) {
                throw new Exception("Send failed");
            }

            while (token.IsCancellationRequested == false)
            { 
                //just keep waiting
                Task.Delay(1000).Wait();
            }

            await dws.DisconnectAsync(token);

            return 1;
        }

        private Task MessageReceived(string receivedMessage)
        {
            messages.Enqueue(receivedMessage);
            _logger.LogInformation(receivedMessage+"\r\n");
            return Task.CompletedTask;
        }

        private Task OrderBookMessageReceived(string receivedMessage)
        {
            try
            {
                if (receivedMessage.StartsWith("{") == false)
                {
                    _logger.LogTrace("Received broken message from the OrderBook stream. This message will be discarded");
                    return Task.CompletedTask;
                } 

                var trimmedEntry = receivedMessage.Remove(receivedMessage.LastIndexOf("}")+1);
                DiffBookDepthStream? orderBookEntry = JsonConvert.DeserializeObject<DiffBookDepthStream>(trimmedEntry);
                if (orderBookEntry != null)
                {
                    if (orderBookEntry.AsksToUpdate == null)
                    { orderBookEntry.AsksToUpdate = new List<BidAskEntry>(); }

                    if (orderBookEntry.BidsToUpdate == null)
                    { orderBookEntry.BidsToUpdate = new List<BidAskEntry>(); }

                    orderBookDiffMessages.Enqueue(orderBookEntry);
                }

                //_logger.LogTrace($"Raw orderBookMessage:{receivedMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Task.FromException(ex);
            }
            return Task.CompletedTask;
        }
    }
}
