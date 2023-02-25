using TradingBot.Models;

namespace TradingBot.TradingServices
{
    public interface ITradesExecutor
    {
        bool Start();
        Task<bool> OpenPosition(string symbol, decimal quantity, decimal price, OrderSide orderSide, OrderType orderType);
        Task<bool> ClosePositionLongAtMarket(string symbol);
        Task<bool> ClosePositionShortAtMarket(string symbol);
    }
}
