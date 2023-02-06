using Newtonsoft.Json;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.Models;

namespace TradingBot.BinanceServices
{
    public class BinanceTradeConnector : ITradeConnector
    {
        public BinanceTradeConnector(ILogger<BinanceTradeConnector> logger, FutureMarket futureMarket)
        {
            this._logger = logger;
            this._futureMarket = futureMarket;
        }


        private ILogger<BinanceTradeConnector> _logger;
        private FutureMarket _futureMarket;

        public async Task<List<BinanceOrder>> GetOpenOrders(string symbol)
        {
            string receivedMessage = await _futureMarket.CurrentOpenOrders(symbol, 5000);
            List<BinanceOrder>? receivedOrders = JsonConvert.DeserializeObject<List<BinanceOrder>>(receivedMessage);
            if (receivedOrders == null)
                throw new InvalidOperationException("Returned Order was null");
            return receivedOrders;
        }

        public async Task<List<BinanceOrder>> GetAllOpenOrders()
        {
            string receivedMessage = await _futureMarket.CurrentOpenOrders(null, 5000);
            List<BinanceOrder>? receivedOrders = JsonConvert.DeserializeObject<List<BinanceOrder>>(receivedMessage);
            if (receivedOrders == null)
                throw new InvalidOperationException("Returned Order was null");
            return receivedOrders;
        }
    }
}
