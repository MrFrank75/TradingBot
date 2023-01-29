﻿using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;

namespace TradingBot.BinanceServices
{
    public interface IOrderBookConverter
    {
        void BuildFromBidAskEntriesStream(ConcurrentQueue<DiffBookDepthStream> orderBookMessages, List<OrderBookEntry> entries, long lastSnapshotUpdate, int priceGranularity);
        void BuildFromSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> entries, int priceGranularity);
    }
}