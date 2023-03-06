using Binance.Spot.Models;
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

        public async Task<BrokerConnectorReturnErrorCode> OpenNewOrder(string symbol, OrderSide side, decimal quantity, decimal price, Models.OrderType orderType)
        {
            _futureMarket.NewOrder(symbol,
                side.FromOrderSide(),
                orderType.FromOrderType(),
                quantity = quantity,
                price = price);
            throw new NotImplementedException();
        }

        public Task<BrokerConnectorReturnErrorCode> OpenNewOrderAtMarket(string symbol, OrderSide side, decimal quantity)
        {
            throw new NotImplementedException();
        }
    }
}


public static class BinanceExtensions
{
    public static Side FromOrderSide(this TradingBot.Models.OrderSide side)
    {
        throw new NotImplementedException();
    }

    public static Binance.Spot.Models.OrderType FromOrderType(this TradingBot.Models.OrderType type)
    {
        throw new NotImplementedException();
    }
}
