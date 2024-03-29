﻿using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;

namespace TradingBot.BinanceServices
{
    public interface IBinanceOrderBookConnector
    {
        ConcurrentQueue<DiffBookDepthStream> OrderBookDiffMessages { get; }

        Task<int> ListenToSingleStream(string stream, CancellationToken token);
        Task<int> ListenToOrderBookDepthStream(string stream, CancellationToken token);
        Task<OrderBookAPISnapshot?> LoadInitialOrderBookSnapshot(string symbol);
        Task<BinanceSymbol> GetSymbolPriceTicker(string symbol);
    }
}
