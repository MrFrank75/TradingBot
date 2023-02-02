namespace TradingBot.Models
{
    public interface ISymbol
    {
        string Ticker { get; }
        double Price { get; }
        DateTime Timestamp { get; }
    }
}