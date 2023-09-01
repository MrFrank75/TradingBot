using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.Models;

namespace TradingBot.TradingServices
{
    public interface IBrokerConnector
    {
        Task<IEnumerable<IOrder>> GetAllOpenOrders();

        Task<IEnumerable<IOrder>> GetOpenOrders(string symbol);
        Task<BrokerConnectorReturnErrorCode> OpenNewOrder(string symbol, OrderSide side, decimal quantity, decimal? price, OrderType orderType);

        Task<BrokerConnectorReturnErrorCode> OpenNewOrderAtMarket(string symbol, OrderSide side, decimal quantity);
    }
}
