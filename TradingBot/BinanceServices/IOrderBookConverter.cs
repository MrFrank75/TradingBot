using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;

namespace TradingBot.BinanceServices
{
    public interface IOrderBookConverter
    {
        void PopulateFromBidAskEntries(ConcurrentQueue<string> orderBookMessages, List<OrderBookEntry> entries);
        void PopulateFromSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> entries, int priceGranularity);
    }
}