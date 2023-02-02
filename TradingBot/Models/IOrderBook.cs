namespace TradingBot.Models
{
    public interface IOrderBook
    {
        Symbol? TickerInfo { get; }
        List<OrderBookEntry> Entries { get; }
    }
}