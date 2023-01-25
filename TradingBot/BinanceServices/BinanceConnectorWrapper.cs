using Binance.Spot;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels;

namespace TradingBot.BinanceServices
{
    public class BinanceConnectorWrapper : IBinanceConnectorWrapper
    {
        private readonly string BaseUrl;
        private readonly ILogger<IBinanceConnectorWrapper> _logger;
        private ConcurrentQueue<string> messages = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> orderBookMessages = new ConcurrentQueue<string>();

        public BinanceConnectorWrapper(ILogger<BinanceConnectorWrapper> logger, string baseUrl = "wss://stream.binancefuture.com")
        {
            this.BaseUrl = baseUrl;
            this._logger = logger;
        }

        public ConcurrentQueue<string> Messages { get => messages; }
        public ConcurrentQueue<string> OrderBookMessages { get => orderBookMessages; }

        public async Task<int> ListenToOrderBook(string stream, CancellationToken token)
        {
            _logger.LogInformation("ListenToOrderBook started");
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
                Task.Delay(1000).Wait();
            }

            await dws.DisconnectAsync(token);

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
                orderBookMessages.Enqueue(receivedMessage);
                var trimmedEntry = receivedMessage.Remove(receivedMessage.LastIndexOf("}")+1);
                DiffBookDepthStream? orderBookEntry = JsonConvert.DeserializeObject<DiffBookDepthStream>(trimmedEntry);

                _logger.LogInformation(receivedMessage + "\r\n");
                _logger.LogInformation(orderBookEntry + "\r\n");
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
