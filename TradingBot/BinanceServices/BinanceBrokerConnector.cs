using Newtonsoft.Json;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.Models;
using TradingBot.TradingServices;

namespace TradingBot.BinanceServices
{
    public class BinanceBrokerConnector : IBrokerConnector
    {
        public BinanceBrokerConnector(ILogger<BinanceBrokerConnector> logger, FutureMarket futureMarket)
        {
            this._logger = logger;
            this._futureMarket = futureMarket;
        }


        private ILogger<BinanceBrokerConnector> _logger;
        private FutureMarket _futureMarket;

        public async Task<IEnumerable<IOrder>> GetOpenOrders(string symbol)
        {
            string receivedMessage = await _futureMarket.CurrentOpenOrders(symbol, 5000);
            List<BinanceOrder>? receivedOrders = JsonConvert.DeserializeObject<List<BinanceOrder>>(receivedMessage);
            if (receivedOrders == null)
                throw new InvalidOperationException("Returned Order was null");

            List<Order> orders = receivedOrders.Select(x => new Order { side = x.side }).ToList();
            return orders;
        }

        public async Task<IEnumerable<IOrder>> GetAllOpenOrders()
        {
            string receivedMessage = await _futureMarket.CurrentOpenOrders(null, 5000);
            List<BinanceOrder>? receivedOrders = JsonConvert.DeserializeObject<List<BinanceOrder>>(receivedMessage);
            if (receivedOrders == null)
                throw new InvalidOperationException("Returned Order was null");

            List<Order> orders = receivedOrders.Select(x => new Order { side = x.side }).ToList();
            return orders;
        }

        public async Task<BrokerConnectorReturnErrorCode> OpenNewOrder(string symbol, OrderSide side, decimal quantity, decimal price, OrderType orderType)
        {
            throw new NotImplementedException();
        }
    }
}
