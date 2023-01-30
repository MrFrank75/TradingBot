using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;
using TradingBot.Models;

namespace TradingBot.BinanceServices
{
    public interface IOrderBookBuilder
    {
        void BuildFromBidAskEntriesStream(ConcurrentQueue<DiffBookDepthStream> orderBookMessages, List<OrderBookEntry> entries, long lastSnapshotUpdate, int priceGranularity);
        void BuildFromSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> entries, int priceGranularity);
    }
}