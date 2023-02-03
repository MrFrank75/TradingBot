using TradingBot.Models;

namespace TradingBot.OrderBook
{
    public interface IOrderBook
    {
        Symbol? TickerInfo { get; }
        List<OrderBookEntry> Entries { get; }

        Task Build(string symbol, CancellationToken cancellationToken);
    }
}