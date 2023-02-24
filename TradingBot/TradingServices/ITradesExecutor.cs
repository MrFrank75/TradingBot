using TradingBot.Models;

namespace TradingBot.TradingServices
{
    public interface ITradesExecutor
    {
        bool Start();
        bool OpenNewOrder(string symbol, OrderSide side, decimal quantity, decimal price, OrderType orderType);
    }
}
