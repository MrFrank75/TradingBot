using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;

namespace TradingBot.BinanceServices
{
    public interface IOrderBookConverter
    {
        void PopulateFromBidAskEntries(ConcurrentQueue<DiffBookDepthStream> orderBookMessages, List<OrderBookEntry> entries);
        void PopulateFromSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> entries, int priceGranularity);
    }
}